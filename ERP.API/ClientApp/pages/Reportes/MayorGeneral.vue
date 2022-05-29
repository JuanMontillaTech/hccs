<script>
import { tsNullKeyword } from "@babel/types";
var numbro = require("numbro");
var moment = require("moment");

/**
 * Invoice Detail component
 */
export default {
  head() {
    return {
      title: `${this.title} `,
    };
  },
  data() {
    return {
      title: "Libro mayor ",
      id: null,
      totalCredit: 0.0,
      totalDebit: 0.0,
      ListBalanceACT: [],
      items: [
        {
          text: "Reporte",
        },
      ],
      company: {},
    };
  },
  created() {
    this.LoadData();
    this.getCompany();
  },
  methods: {
    calculateTotal() {
      this.totalCredit = this.ListBalanceACT.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.credit);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);
      this.totalDebit = this.ListBalanceACT.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.debit);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);
    },
    LoadData() {
      let url =
        this.$store.state.URL + `Journal/GetAllLedgerAccountByCode?Code=MY`;

      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          console.log(response.data.data);
          this.ListBalanceACT = response.data.data;
          this.calculateTotal();
        })
        .catch((error) => {
          console.log(error);
        });
    },
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    GetDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },
    getCompany() {
      let url = this.$store.state.URL + `Company/GetDefault`;

      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.company = response.data.data;
        })
        .catch((error) => {
          console.log(error);
        });
    },
  },
};
</script>

<template>
  <div>
    <PageHeader :items="items" />

    <div class="row">
      <div class="col-lg-12">
        <div class="card">
          <div class="card-body">
            <CompanyHead title="Libro mayor"></CompanyHead>

            <hr class="my-4" />

            <div class="py-2">
              <div class="table-responsive">
                <div v-if="ListBalanceACT.length == 0">
              
                  <div class="spinner">
                    <i class="uil-shutter-alt spin-icon"></i>
                  </div>
                </div>
                <table
                  v-if="ListBalanceACT.length > 0"
                  class="table table-nowrap table-centered mb-0"
                >
                  <thead>
                    <tr>
                      <th>Cuenta</th>
                      <th class="text-right" style="width: 120px">Debito</th>
                      <th class="text-right" style="width: 120px">Credito</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr
                      v-for="row in ListBalanceACT"
                      :key="row.ledgerAccountId"
                    >
                      <td>
                        <h5 class="font-size-14 mb-1">
                          {{ row.code }} {{ row.name }}
                        </h5>
                      </td>
                      <td class="border-0 text-right">
                        {{ SetTotal(row.credit) }}
                      </td>
                      <td class="border-0 text-right">
                        {{ SetTotal(row.debit) }}
                      </td>
                    </tr>

                    <tr>
                      <th>
                        <h5 class="font-size-14 mb-1">Total</h5>
                      </th>
                      <td class="border-0 text-right doubleLine">
                        <div class="doubleLine">
                          {{ SetTotal(totalCredit) }}
                        </div>
                      </td>
                      <td class="border-0 text-right doubleLine">
                        <div class="doubleLine">{{ SetTotal(totalDebit) }}</div>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
              <div class="d-print-none mt-4">
                <print></print>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- end row -->
  </div>
</template>
<style>
.doubleLine {
  border-top: double;
}
</style>

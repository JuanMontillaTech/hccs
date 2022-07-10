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
      title: "Balance de comprobación ",
      id: null,
      ListTotal:[],
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
  },
  methods: {
   SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    TotalList(type) {
      var dataFillter = this.ListBalanceACT .filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);
   
      return  this.totalbalance;
    },
    LoadData() {
      let url =
         `Journal/GetAllLedgerAccountByCode?Code=EST`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT = response.data.data;
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
            <CompanyHead title="Estado de resultado"></CompanyHead>
            <hr class="my-4" />

            <div class="py-2">
              <div class="table-responsive">
                <h3>Ingresos</h3>
                <BalanceCriterio
                  :List="ListBalanceACT"
                  :type="1"
                ></BalanceCriterio>
                 <h2>Gastos</h2>
                <BalanceCriterio
                  :List="ListBalanceACT"
                  :type="2"
                ></BalanceCriterio>
                <Table class="table table-nowrap table-centered mb-0">
                  <tbody>
                    <tr>
                      <td class="font-size-14 mb-1"><h5>Total Utilidad</h5></td>
                      <td   style="width: 120px" class="text-right">
                      <div class="doubleLine">{{SetTotal(TotalList(1) - TotalList(2))}}</div>
                          
                       
                      </td>
                    </tr>
                  </tbody>
                </Table>
          
                
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

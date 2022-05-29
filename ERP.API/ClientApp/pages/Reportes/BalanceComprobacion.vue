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
            <CompanyHead title="Balance de comprobación"></CompanyHead>
            <hr class="my-4" />

            <div class="py-2">
              <div class="table-responsive">
                <h3>Activos</h3>
                <BalanceCriterio
                  :List="ListBalanceACT"
                  :type="1"
                ></BalanceCriterio>
                 <h2>Pasivos</h2>
                <BalanceCriterio
                  :List="ListBalanceACT"
                  :type="2"
                ></BalanceCriterio>
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

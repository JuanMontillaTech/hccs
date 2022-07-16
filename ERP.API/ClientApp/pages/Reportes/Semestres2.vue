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
      ListTotal: [],
      ListBalanceACT1: [],
      ListBalanceACT2: [],
      ListBalanceACT3: [],
      ListBalanceACT4: [],
      ListBalanceACT5: [],
      ListBalanceACT6: [],
      items: [
        {
          text: "Reporte",
        },
      ],
      company: {},
    };
  },
  created() {
    this.LoadData1();
    this.LoadData2();
    this.LoadData3();
    this.LoadData4();
    this.LoadData5();
    this.LoadData6();
  },
  methods: {
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    TotalList(type) {
      var dataFillter = this.ListBalanceACT1.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    TotalList1(type) {
      var dataFillter = this.ListBalanceACT1.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    TotalList2(type) {
      var dataFillter = this.ListBalanceACT2.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    TotalList3(type) {
      var dataFillter = this.ListBalanceACT3.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    TotalList4(type) {
      var dataFillter = this.ListBalanceACT4.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    TotalList5(type) {
      var dataFillter = this.ListBalanceACT5.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    TotalList6(type) {
      var dataFillter = this.ListBalanceACT6.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    LoadData1() {
      let url =
        `Journal/GetAllLedgerAccountByCodeMonth?Code=EST&Month=7`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT1 = response.data.data;
        })
        .catch((error) => {
          console.log(error);
        });
    },
    LoadData2() {
      let url =
        `Journal/GetAllLedgerAccountByCodeMonth?Code=EST&Month=8`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT2 = response.data.data;
        })
        .catch((error) => {
          console.log(error);
        });
    },
    LoadData3() {
      let url =
        `Journal/GetAllLedgerAccountByCodeMonth?Code=EST&Month=9`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT3 = response.data.data;
        })
        .catch((error) => {
          console.log(error);
        });
    },
    LoadData4() {
      let url =
        `Journal/GetAllLedgerAccountByCodeMonth?Code=EST&Month=10`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT4 = response.data.data;
        })
        .catch((error) => {
          console.log(error);
        });
    },
    LoadData5() {
      let url =
        `Journal/GetAllLedgerAccountByCodeMonth?Code=EST&Month=11`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT5 = response.data.data;
        })
        .catch((error) => {
          console.log(error);
        });
    },
    LoadData6() {
      let url =
        `Journal/GetAllLedgerAccountByCodeMonth?Code=EST&Month=12`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT6 = response.data.data;
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


                <table class="table">

                  <tbody>
                    <tr>
                      <th>Nombre de Cuenta </th>
                      <th>JULIO</th>
                      <th>AGOSTO</th>
                      <th>SEPTIEMBRE</th>
                      <th>OBTUBRE</th>
                      <TH> NOVIEMBRE</TH>
                      <th>DICIEMBRE</th>

                    </tr>
                    <tr>
                      <th>Caja Disponible actual </th>
                      <td>
                        {{ SetTotal(TotalList1(1) -TotalList1(2) ) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList2(1) -TotalList2(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList3(1) -TotalList3(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList4(1) -TotalList4(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList5(1) -TotalList5(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList6(1) -TotalList5(2)) }}
                      </td>

                    </tr>
                    <tr>
                      <th>
                        <h4>4 INGRESOS </h4>
                      </th>
                      <td> </td>
                      <td> </td>
                      <td> </td>
                      <td> </td>
                      <td> </td>

                    </tr>
                    <tr>
                      <Td>
                        <BalanceCriterioHead :List="ListBalanceACT1" :type="1"></BalanceCriterioHead>
                      </Td>
                      <td>
                        <BalanceCriterioValue :List="ListBalanceACT1" :type="1"></BalanceCriterioValue>
                      </td>
                      <td>
                        <BalanceCriterioValue :List="ListBalanceACT2" :type="1"></BalanceCriterioValue>
                      </td>
                      <td>
                        <BalanceCriterioValue :List="ListBalanceACT3" :type="1"></BalanceCriterioValue>
                      </td>
                      <td>
                        <BalanceCriterioValue :List="ListBalanceACT4" :type="1"></BalanceCriterioValue>
                      </td>
                      <td>
                        <BalanceCriterioValue :List="ListBalanceACT5" :type="1"></BalanceCriterioValue>
                      </td>
                      <td>
                        <BalanceCriterioValue :List="ListBalanceACT6" :type="1"></BalanceCriterioValue>
                      </td>
                    </tr>

                  </tbody>
                  <tfoot>
                    <tr>
                      <Th>
                        Total Ingresos + Caja
                      </Th>
                      <td>
                        {{ SetTotal(TotalList1(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList2(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList3(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList4(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList5(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList5(2)) }}
                      </td>
                    </tr>
                  </tfoot>
                </table>
                <table>

                  <tbody>



                  </tbody>
                </table>
                <table class="table">

                  <tbody>

                    <tr>
                      <th>
                        <h4>6 EGRESOS </h4>
                      </th>
                      <td> </td>
                      <td> </td>
                      <td> </td>
                      <td> </td>
                      <td> </td>

                    </tr>
                    <tr>
                      <Td>
                        <BalanceCriterioHead :List="ListBalanceACT1" :type="2"></BalanceCriterioHead>
                      </Td>
                      <td>
                        <BalanceCriterioValue :List="ListBalanceACT1" :type="2"></BalanceCriterioValue>
                      </td>
                      <td>
                        <BalanceCriterioValue :List="ListBalanceACT2" :type="2"></BalanceCriterioValue>
                      </td>
                      <td>
                        <BalanceCriterioValue :List="ListBalanceACT3" :type="2"></BalanceCriterioValue>
                      </td>
                      <td>
                        <BalanceCriterioValue :List="ListBalanceACT4" :type="2"></BalanceCriterioValue>
                      </td>
                      <td>
                        <BalanceCriterioValue :List="ListBalanceACT5" :type="2"></BalanceCriterioValue>
                      </td>
                      <td>
                        <BalanceCriterioValue :List="ListBalanceACT6" :type="2"></BalanceCriterioValue>
                      </td>
                    </tr>

                  </tbody>
                  <tfoot>
                    <tr>
                      <Th>
                        Total Egresos + Caja
                      </Th>
                      <td>
                        {{ SetTotal(TotalList1(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList2(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList3(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList4(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList5(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList5(2)) }}
                      </td>
                    </tr>
                  </tfoot>
                </table>
                <table>

                  <tbody>



                  </tbody>
                </table>
                <table class="table">
                  <tbody>
                    <tr>
                      <Th>
                        SUMA TOTAL DE INGRESOS
                      </Th>
                      <td>
                        {{ SetTotal(TotalList1(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList2(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList3(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList4(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList5(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList5(2)) }}
                      </td>
                    </tr>
                    <tr>
                      <Th>
                        SUMA TOTAL DE EGRESOS
                      </Th>
                      <td>
                        {{ SetTotal(TotalList1(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList2(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList3(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList4(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList5(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList5(2)) }}
                      </td>
                    </tr>
                       <tr>
                      <Th>
                        CAJA DISPONIBLE O DIFERENCIA
                      </Th>
                      <td>
                        {{ SetTotal(TotalList1(1) -TotalList1(2) ) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList2(1) -TotalList2(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList3(1) -TotalList3(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList4(1) -TotalList4(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList5(1) -TotalList5(2)) }}
                      </td>
                      <td>
                        {{ SetTotal(TotalList6(1) -TotalList5(2)) }}
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

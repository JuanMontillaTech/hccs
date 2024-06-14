<script>
var numbro = require("numbro");
var moment = require("moment");
import * as XLSX from "xlsx"; // Importar XLSX
/**
 * Invoice Detail component
 */
export default {
  layout: "auth",
  head() {
    return {
      title: `${this.title} `,
    };
  },
  data() {
    return {
      ReportData: [],
      FormId: "dce77e36-7f8b-4f48-a0bb-a1490028c94d",
      FormId2: "9f18d49a-f4ee-4a08-b402-c664cd8471c3",
      principalSchema: { date: null },
      title: "Balance de comprobación",
      id: null,
      Boxs: {
        Box0: 0,
        Box1: 0,
        Box2: 0,
        Box3: 0,
        Box4: 0,
        Box5: 0,
        Box6: 0,
        value: 0,
        Box7: 0,
      },
      Balances2: [],
      Balances: [],

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

  created() {},
  methods: {
    exportarExcel() {
      // Obtener el componente b-table a través de la referencia y luego acceder al elemento DOM
      const table = this.$refs.miTabla.$el;
      this.FileName = `Reporte1`.replace(/\s/g, "_");

      // Crear una hoja de trabajo a partir del elemento DOM de la tabla
      const worksheet = XLSX.utils.table_to_sheet(table);

      // Crear un libro de trabajo y agregar la hoja de trabajo
      const workbook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(workbook, worksheet, "Reporte");

      // Generar el archivo Excel
      XLSX.writeFile(workbook, this.FileName + ".xlsx");
    },
    filtrarPorTipo(datos, tipo) {
      return datos.filter((dato) => dato.tipo === tipo );
    },
    getReport() {
      let data = JSON.stringify(this.principalSchema);
      let url = `Report/GetById?id=${this.FormId}&Data=${data}`;
      this.getBox();
      this.$axios
        .get(url)
        .then((response) => {
          this.ReportData = response.data.data;
          console.log(this.ReportData);

          var ingresos = this.filtrarPorTipo(this.ReportData, "Ingreso");
          var egresos = this.filtrarPorTipo(this.ReportData, "Gasto");
          this.Balances = this.transformDataToBalances(ingresos);
          this.Balances2 = this.transformDataToBalances(egresos);
        })
        .catch((error) => {});
    },

    getBox() {
      let url = `BoxBalance/GetByYear?year=${this.principalSchema.date}`;

      this.$axios
        .get(url)
        .then((response) => {
          this.Boxs.Box0 = response.data.data.balance;
        })
        .catch((error) => {});
    },

    transformDataToBalances(data) {
      const balances = [];

      data.forEach((item) => {
        // Encontrar o crear el balance correspondiente en el arreglo
        let balance = balances.find((b) => b.Name === item.cuenta);
        if (!balance) {
          balance = {
            Code: item.codigo, // Puedes ajustar este código según tus necesidades
            Name: item.cuenta,
            Months: [],
            TotalMonth: 0,
          };
          balances.push(balance);
        }

        // Agregar o actualizar el mes en el balance
        const monthIndex = item.mes - 1; // Los índices del arreglo comienzan en 0
        if (balance.Months[monthIndex]) {
          balance.Months[monthIndex].value += item.Total;
        } else {
          balance.Months[monthIndex] = { month: item.mes, value: item.Total };
        }
        balance.TotalMonth += item.Total;
      });

      // Llenar los meses faltantes con valor 0
      balances.forEach((balance) => {
        for (let i = 0; i < 12; i++) {
          if (!balance.Months[i]) {
            balance.Months[i] = { month: i + 1, value: 0 };
          }
        }
      });

      return balances;
    },
    getTotalForCode(data, code) {
      let total = 0;
      for (const balance of data) {
        if (balance.Code === code) {
          for (const month of balance.Months) {
            total += month.value;
          }
        }
      }
      return total;
    },
    getAllTotalForCode(data) {
      let total = 0;
      for (const balance of data) {
        for (const month of balance.Months) {
          total += month.value;
        }
      }
      return total;
    },

    getMonthValue(monthNumber, Months) {
      const monthData = Months.find((month) => month.month === monthNumber);
      if (monthData) {
        return monthData.value;
      } else {
        return 0.0;
      }
    },
    getTotalForMonth(Month, Balances) {
      let totalMonth1 = 0;

      for (const balance of Balances) {
        totalMonth1 += this.getMonthValue(Month, balance.Months);
      }

      return totalMonth1;
    },
    UpDateBox(NumberBox, TotalIncomin, TotalOutComin) {
      var outValue = 0.0;
      switch (NumberBox) {
        case 1:
          var GTotalIncomin = TotalIncomin + this.Boxs.Box0;
          outValue = GTotalIncomin - TotalOutComin;
          this.Boxs.Box1 = outValue;
          break;
        case 2:
          var GTotalIncomin = TotalIncomin + this.Boxs.Box1;
          outValue = GTotalIncomin - TotalOutComin;
          this.Boxs.Box2 = outValue;
          break;
        case 3:
          var GTotalIncomin = TotalIncomin + this.Boxs.Box2;
          outValue = GTotalIncomin - TotalOutComin;
          this.Boxs.Box3 = outValue;
          break;
        case 4:
          var GTotalIncomin = TotalIncomin + this.Boxs.Box3;
          outValue = GTotalIncomin - TotalOutComin;
          this.Boxs.Box4 = outValue;
          break;
        case 5:
          var GTotalIncomin = TotalIncomin + this.Boxs.Box4;
          outValue = GTotalIncomin - TotalOutComin;
          this.Boxs.Box5 = outValue;
          break;
        case 6:
          var GTotalIncomin = TotalIncomin + this.Boxs.Box5;
          outValue = GTotalIncomin - TotalOutComin;
          this.Boxs.Box6 = outValue;
          break;
      }
      return outValue;
    },
    GoBack() {
      this.$router.push({ path: `/starter` });
    },

    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    TotalBox() {
      var TBoxes =
        this.Boxs.Box0 +
        this.Boxs.Box1 +
        this.Boxs.Box3 +
        this.Boxs.Box4 +
        this.Boxs.Box5 +
        this.Boxs.Box6;
      return TBoxes;
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

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
      FormId: "9f18d49a-f4ee-4a08-b402-c664cd8471c2",
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
        Box7: 0,
        Box8: 0,
        Box9: 0,
        Box10: 0,
        Box11: 0,
        Box12: 0,
        value: 0,
      },
      Balances2: [],
      Balances: [],

      ListTotal: [],

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
      this.FileName = `Reporte ${this.DataForm.title}`.replace(/\s/g, "_");

      // Crear una hoja de trabajo a partir del elemento DOM de la tabla
      const worksheet = XLSX.utils.table_to_sheet(table);

      // Crear un libro de trabajo y agregar la hoja de trabajo
      const workbook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(workbook, worksheet, "Reporte");

      // Generar el archivo Excel
      XLSX.writeFile(workbook, this.FileName + ".xlsx");
    },
    filtrarPorTipo(datos, tipo) {
      return datos.filter((dato) => dato.tipo === tipo);
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
      
        case 7:
          var GTotalIncomin = TotalIncomin + this.Boxs.Box6;
          outValue = GTotalIncomin - TotalOutComin;
          this.Boxs.Box7 = outValue;
          break;
        case 8:
          var GTotalIncomin = TotalIncomin + this.Boxs.Box7;
          outValue = GTotalIncomin - TotalOutComin;
          this.Boxs.Box8 = outValue;
          break;
        case 9:
          var GTotalIncomin = TotalIncomin + this.Boxs.Box8;
          outValue = GTotalIncomin - TotalOutComin;
          this.Boxs.Box9 = outValue;
          break;
          case 10:
          var GTotalIncomin = TotalIncomin + this.Boxs.Box9;
          outValue = GTotalIncomin - TotalOutComin;
          this.Boxs.Box10 = outValue;
          break;
        case 11:
          var GTotalIncomin = TotalIncomin + this.Boxs.Box10;
          outValue = GTotalIncomin - TotalOutComin;
          this.Boxs.Box11 = outValue;
          break;
        case 12:
          var GTotalIncomin = TotalIncomin + this.Boxs.Box11;
          outValue = GTotalIncomin - TotalOutComin;
          this.Boxs.Box12= outValue;
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
        this.Boxs.Box6 +
        this.Boxs.Box7 +
        this.Boxs.Box8 +
        this.Boxs.Box9 +
        this.Boxs.Box10 +
        this.Boxs.Box11 +
        this.Boxs.Box12;
      return TBoxes;
    },
  },
};
</script>

<template>
  <div class="font-size-12">
    <div class="row">
      <div class="col-lg-12">
        <div class="card">
          <div class="card-header" style="background-color: white">
            <div class="row">
              <div class="col-sm-04">
                <b-button
                  variant="secundary"
                  class="btn d-print-none mt-4"
                  @click="GoBack()"
                >
                  <i class="bx bx-arrow-back"></i> Regresar
                </b-button>
              </div>
              <div class="col-sm-04">
                <p>Fecha</p>
              </div>
              <div class="col-sm-04">
                <b-form-input
                  class="d-print-none"
                  v-model="principalSchema.date"
                  type="date"
                  size="sm"
                  w-100
                  style="width: 150px"
                ></b-form-input>
              </div>
              <div class="col-sm-04">
                <b-button
                  variant="primary"
                  size="sm"
                  class="sm btn d-print-none mt-4"
                  @click="getReport()"
                >
                  Buscar
                </b-button>
              </div>
            </div>
            <Companyinfo
              class="text-center"
              title="Estado de resultado"
            ></Companyinfo>
          </div>
          <div class="card-body">
            <hr class="my-4" />
            <table class="d-print-none">
              <td><label>Fecha </label></td>

              <td></td>
            </table>

            <CompanyRpHead
              class="text-center"
              title="Estado de resultado"
            ></CompanyRpHead>

            <div>
              <div class="table-responsive">
                <table class="w-100" ref="miTabla">
                  <tbody>
                    <tr>
                      <th>Nombre de Cuenta</th>
                      <td></td>
                      <th>ENERO</th>
                      <th>FEBRERO</th>
                      <th>MARZO</th>
                      <th>ABRIL</th>
                      <TH>MAYO</TH>
                      <th>JUNIO</th>
                      <th>JULIO</th>
                      <th>AGOSTO</th>
                      <th>SEPTIEMBRE</th>
                      <th>OBTUBRE</th>
                      <TH> NOVIEMBRE</TH>
                      <th>DICIEMBRE</th>
                      <th>Total de Gastos</th>
                    </tr>
                    <tr>
                      <th>Caja Disponible actual</th>

                      <td></td>
                      <td>
                        {{ SetTotal(Boxs.Box0) }}
                      </td>
                      <td>
                        {{ SetTotal(Boxs.Box1) }}
                      </td>
                      <td>
                        {{ SetTotal(Boxs.Box2) }}
                      </td>
                      <td>
                        {{ SetTotal(Boxs.Box3) }}
                      </td>
                      <td>
                        {{ SetTotal(Boxs.Box4) }}
                      </td>
                      <td>
                        {{ SetTotal(Boxs.Box5) }}
                      </td>
                      <td>
                        {{ SetTotal(Boxs.Box6) }}
                      </td>
                      <td>
                        {{ SetTotal(Boxs.Box7) }}
                      </td>
                      <td>
                        {{ SetTotal(Boxs.Box8) }}
                      </td>
                      <td>
                        {{ SetTotal(Boxs.Box9) }}
                      </td>
                      <td>
                        {{ SetTotal(Boxs.Box10) }}
                      </td>
                      <td>
                        {{ SetTotal(Boxs.Box11) }}
                      </td>
                  
                      <td>
                        {{ SetTotal(Boxs.Box0) }}
                      </td>
                    </tr>
                    <tr class="font-size-15">
                      <th>41</th>
                      <th>INGRESOS</th>
                      <td></td>
                      <td></td>
                      <td></td>
                      <td></td>
                      <td></td>
                      <td></td>
                      <td></td>
                      <td></td>
                      <td></td>
                      <td></td>
                      <td></td>
                      <td></td>
                      <td></td>
                    </tr>
                    <tr v-for="row in Balances">
                      <Td> {{ row.Code }} </Td>
                      <td>{{ row.Name }}</td>
                      <td>
                        {{ SetTotal(getMonthValue(1, row.Months)) }}
                      </td>
                      <td>
                        {{ SetTotal(getMonthValue(2, row.Months)) }}
                      </td>
                      <td>
                        {{ SetTotal(getMonthValue(3, row.Months)) }}
                      </td>
                      <td>
                        {{ SetTotal(getMonthValue(4, row.Months)) }}
                      </td>
                      <td>
                        {{ SetTotal(getMonthValue(5, row.Months)) }}
                      </td>
                      <td>
                        {{ SetTotal(getMonthValue(6, row.Months)) }}
                      </td>
                      <td>
                        {{ SetTotal(getMonthValue(7, row.Months)) }}
                      </td>
                      <td>
                        {{ SetTotal(getMonthValue(8, row.Months)) }}
                      </td>
                      <td>
                        {{ SetTotal(getMonthValue(9, row.Months)) }}
                      </td>
                      <td>
                        {{ SetTotal(getMonthValue(10, row.Months)) }}
                      </td>
                      <td>
                        {{ SetTotal(getMonthValue(11, row.Months)) }}
                      </td>
                      <td>
                        {{ SetTotal(getMonthValue(12, row.Months)) }}
                      </td>
                      <td>
                        {{ SetTotal(getTotalForCode(Balances, row.Code)) }}
                      </td>
                    </tr>
                  </tbody>

                  <tr>
                    <Th> Total de Ingresos del Mes </Th>
                    <Th> </Th>
                    <td>
                      {{ SetTotal(getTotalForMonth(1, Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(2, Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(3, Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(4, Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(5, Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(6, Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(7, Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(8, Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(9, Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(10, Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(11, Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(12, Balances)) }}
                    </td>

                    <td>
                      {{
                        SetTotal(
                          getTotalForMonth(1, Balances) +
                            getTotalForMonth(2, Balances) +
                            getTotalForMonth(3, Balances) +
                            getTotalForMonth(4, Balances) +
                            getTotalForMonth(5, Balances) +
                            getTotalForMonth(6, Balances) +
                            getTotalForMonth(7, Balances) +
                            getTotalForMonth(8, Balances) +
                            getTotalForMonth(9, Balances) +
                            getTotalForMonth(10, Balances) +
                            getTotalForMonth(11, Balances) +
                            getTotalForMonth(12, Balances)
                        )
                      }}
                    </td>
                  </tr>
                  <tr>
                    <Th> Total de Ingresos + Caja </Th>
                    <Th> </Th>
                    <td>
                      {{ SetTotal(getTotalForMonth(1, Balances) + Boxs.Box0) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(2, Balances) + Boxs.Box1) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(3, Balances) + Boxs.Box2) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(4, Balances) + Boxs.Box3) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(5, Balances) + Boxs.Box4) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(6, Balances) + Boxs.Box5) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(7, Balances) + Boxs.Box6) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(8, Balances) + Boxs.Box7) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(9, Balances) + Boxs.Box8) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(10, Balances) + Boxs.Box9) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(11, Balances) + Boxs.Box10) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(12, Balances) + Boxs.Box11) }}
                    </td>
                    <td>
                      {{SetTotal( Boxs.Box0 + getAllTotalForCode(Balances)) }}
                    </td>
                  </tr>

                  <tr>
                    <th>61</th>
                    <th>EGRESOS / SALIDAS</th>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                  </tr>
                  <tr v-for="row in Balances2" :key="row.Code">
                    <Td>
                      {{ row.Code }}
                    </Td>
                    <td>
                      {{ row.Name }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(1, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(2, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(3, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(4, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(5, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(6, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(7, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(8, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(9, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(10, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(11, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(12, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForCode(Balances2, row.Code)) }}
                    </td>
                  </tr>

                  <tr>
                    <Th> Total de Egresos </Th>
                    <Th> </Th>
                    <td>
                      {{ SetTotal(getTotalForMonth(1, Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(2, Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(3, Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(4, Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(5, Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(6, Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(7, Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(8, Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(9, Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(10, Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(11, Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(12, Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getAllTotalForCode(Balances2)) }}
                    </td>
                  </tr>
                  <tr>
                    <Th> Total de Ingresos </Th>
                    <Th> </Th>
                    <td>
                      {{ SetTotal(getTotalForMonth(1, Balances) + Boxs.Box0) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(2, Balances) + Boxs.Box1) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(3, Balances) + Boxs.Box3) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(4, Balances) + Boxs.Box4) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(5, Balances) + Boxs.Box5) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(6, Balances) + Boxs.Box6) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(7, Balances) + Boxs.Box7) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(8, Balances) + Boxs.Box8) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(9, Balances) + Boxs.Box9) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(10, Balances) + Boxs.Box10) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(11, Balances) + Boxs.Box11) }}
                    </td>
                    <td>
                      {{ SetTotal(
                        getTotalForMonth(12, Balances) + Boxs.Box12
                      ) }}
                    </td>
                    
                    <td>
                      {{  SetTotal(Boxs.Box0 + getAllTotalForCode(Balances) )}}
                    </td>
                  </tr>
                  <tr>
                    <Th> Caja Disponible o Diferencia </Th>
                    <Th> </Th>
                    <td>
                      {{
                        SetTotal(
                          UpDateBox(
                            1,
                            getTotalForMonth(1, Balances),
                            getTotalForMonth(1, Balances2)
                          )
                        )
                      }}
                    </td>
                    <td>
                      {{
                        SetTotal(
                          UpDateBox(
                            2,
                            getTotalForMonth(2, Balances),
                            getTotalForMonth(2, Balances2)
                          )
                        )
                      }}
                    </td>
                    <td>
                      {{
                        SetTotal(
                          UpDateBox(
                            3,
                            getTotalForMonth(3, Balances),
                            getTotalForMonth(3, Balances2)
                          )
                        )
                      }}
                    </td>
                    <td>
                      {{
                        SetTotal(
                          UpDateBox(
                            4,
                            getTotalForMonth(4, Balances),
                            getTotalForMonth(4, Balances2)
                          )
                        )
                      }}
                    </td>
                    <td>
                      {{
                        SetTotal(
                          UpDateBox(
                            5,
                            getTotalForMonth(5, Balances),
                            getTotalForMonth(5, Balances2)
                          )
                        )
                      }}
                    </td>
                    <td>
                      {{
                        SetTotal(
                          UpDateBox(
                            6,
                            getTotalForMonth(6, Balances),
                            getTotalForMonth(6, Balances2)
                          )
                        )
                      }}
                    </td>
                    <td>
                      {{
                        SetTotal(
                          UpDateBox(
                            7,
                            getTotalForMonth(7, Balances),
                            getTotalForMonth(7, Balances2)
                          )
                        )
                      }}
                    </td>
                    <td>
                      {{
                        SetTotal(
                          UpDateBox(
                            8,
                            getTotalForMonth(8, Balances),
                            getTotalForMonth(8, Balances2)
                          )
                        )
                      }}
                    </td>
                    <td>
                      {{
                        SetTotal(
                          UpDateBox(
                            9,
                            getTotalForMonth(9, Balances),
                            getTotalForMonth(9, Balances2)
                          )
                        )
                      }}
                    </td>
                    <td>
                      {{
                        SetTotal(
                          UpDateBox(
                            10,
                            getTotalForMonth(10, Balances),
                            getTotalForMonth(10, Balances2)
                          )
                        )
                      }}
                    </td>
                    <td>
                      {{
                        SetTotal(
                          UpDateBox(
                            11,
                            getTotalForMonth(11, Balances),
                            getTotalForMonth(11, Balances2)
                          )
                        )
                      }}
                    </td>
                    <td>
                      {{
                        SetTotal(
                          UpDateBox(
                            12,
                            getTotalForMonth(12, Balances),
                            getTotalForMonth(12, Balances2)
                          )
                        )
                      }}
                    </td>
                    <td>
                      {{
                        SetTotal(
                          Boxs.Box0 +
                            getAllTotalForCode(Balances) -
                            getAllTotalForCode(Balances2)
                        )
                      }}
                    </td>
                  </tr>
                </table>
              </div>

              <b-button
                variant="secundary"
                class="btn d-print-none mt-4"
                @click="GoBack()"
              >
                <i class="bx bx-arrow-back"></i> Regresar
              </b-button>

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

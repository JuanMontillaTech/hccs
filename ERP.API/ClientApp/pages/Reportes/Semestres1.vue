<script>
var numbro = require("numbro");
var moment = require("moment");

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
      principalSchema: {},
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
      Balances2: [
         
      ],
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
  
  created() {
    
    this.getReport();
  },
  methods: {
    getReport() {
      let data = JSON.stringify(this.principalSchema);
      let url = `Report/GetById?id=${this.FormId}&Data=${data}`;
 

      this.$axios
        .get(url)
        .then((response) => {
          this.ReportData = response.data.data;
          console.log(this.ReportData);
        
          this.Balances = this.transformDataToBalances(this.ReportData);
          getReport2() 
           
        })
        .catch((error) => {});
    },
    getReport2() {
      let data = JSON.stringify(this.principalSchema);
      let url = `Report/GetById?id=${this.FormId2}&Data=${data}`;


      this.$axios
        .get(url)
        .then((response) => {
          this.ReportData = response.data.data;

          this.Balances2 = this.transformDataToBalances(this.ReportData);

        })
        .catch((error) => { });
    },
    transformDataToBalances(data) {
      const balances = [];

      data.forEach((item) => {
        // Encontrar o crear el balance correspondiente en el arreglo
        let balance = balances.find((b) => b.Name === item.cuenta);
        if (!balance) {
          balance = {
            Code: "0", // Puedes ajustar este código según tus necesidades
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
  <div class="font-size-12">
    <div class="row">
      <div class="col-lg-12">
        <div class="card">
          <div class="card-body">
            <CompanyHead
              class="text-center"
              title="Estado de resultado"
            ></CompanyHead>

            <b-button
              variant="secundary"
              class="btn d-print-none mt-4"
              @click="GoBack()"
            >
              <i class="bx bx-arrow-back"></i> Regresar
            </b-button>
            <hr class="my-4" />
            <CompanyRpHead
              class="text-center"
              title="Estado de resultado"
            ></CompanyRpHead>
          
            <div class="py-2">
              <div class="table-responsive">
               
                <table class="w-100" >
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
                    </tr>
                    <tr v-for="row in Balances"  >
                      <Td>
                       codigo {{ row.Code }}
                      </Td>
                      <td>
                       cuenta {{ row.Name }}
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
                      {{
                        SetTotal(
                          getTotalForMonth(1, Balances) +
                            getTotalForMonth(2, Balances) +
                            getTotalForMonth(3, Balances) +
                            getTotalForMonth(4, Balances) +
                            getTotalForMonth(5, Balances) +
                            getTotalForMonth(6, Balances)
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
                      {{ Boxs.Box0 + getAllTotalForCode(Balances) }}
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
                      {{ Boxs.Box0 + getAllTotalForCode(Balances) }}
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

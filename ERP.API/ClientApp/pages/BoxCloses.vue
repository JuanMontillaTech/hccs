<template>
  <div>
    <div>
      <div class="d-print-none card">
        <div class="card-body">
          <div class="row">
            <div class="col-4">
              <h4 class="card-title">Fecha inicial</h4>
              <b-form-datepicker
                v-model="startDate"
                locale="es"
                :disabled="$route.query.Action == 'show'"
                class="mb-2"
              ></b-form-datepicker>
            </div>
            <div class="col-4">
              <h4 class="card-title">Fecha Final</h4>
              <b-form-datepicker
                v-model="endDate"
                locale="es"
                :disabled="$route.query.Action == 'show'"
                class="mb-2"
              ></b-form-datepicker>
            </div>
          </div>
          <div class="row">
            <div class="float-end">
              <a
                href="javascript:window.print()"
                class="btn btn-success waves-effect waves-light mr-1"
              >
                <i class="fa fa-print"></i> </a
              ><b-button variant="primary" class="btn" @click="GoNew()">
                <i class="mdi mdi-plus me-1"></i> Buscar
              </b-button>
            </div>
          </div>
        </div>
      </div>

      <div class="d-print-none mt-4 text-center">
        Vista de factura para imprimir
      </div>

      <div
        class="ticket"
        style="font: 14px Lucida Console; background-color: white; color: black"
      >
        {{ company.companyName }}
        {{PaymentSchema}}
        </br>
{{FormatDate(startDate)}} -- {{FormatDate(endDate)}}
        <br />
        <div v-for="(PaymentRow, Paymenindex) in PaymentSchema" :key="Paymenindex">
 
        <table class="table table-nowrap table-centered mb-0" v-if="Box6">
          <thead>
            <tr>
              <th>{{PaymentRow.name}} </th>
            </tr>
          </thead>
          <tbody>
             
            <tr v-for="(row6, index) in Box6" :key="index">
              <td>
              
                <span class="font-size-14 mb-1" v-if="row6.paymentMethodId == PaymentRow.id ">
                  {{ row6.code }}
                  {{  row6.globalTotal  }}
                </span>
              </td>
            </tr>
          </tbody>
        </table>
        </div>
        {{Box6}}
        <table class="table table-nowrap table-centered mb-0" v-if="Box5">
          <thead>
            <tr>
              <th>Factura a Credito</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row6, index) in Box5" :key="index">
              <td>
                <h5 class="font-size-14 mb-1">
                  {{ row6.code }} {{ row6.paymentMethods.name }}
                  {{ SetTotal(row6.globalTotal) }}
                </h5>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
var numbro = require("numbro");
var moment = require("moment");
export default {
  head() {
    return {
      title: `Salida de impresora | ERP`,
    };
  },
  data() {
    return {
      FormId: "",
      PageCreate: "/ExpressForm/FuncionalFormExpress",
      company: [],
      startDate: "",
      endDate: "",
      Box5: [],
      Box6: [],
 
      PaymentSchema: [],
    };
  },

  //middleware: "authentication",
  created() {
    this.getCompany();
    this.GetPaymentMethod();
  },

  methods: {
    getCompany() {
      let url = `Company/GetDefault`;
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
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },

    GetPaymentMethod() {
      let url = `PaymentMethod/GetAll`;

      this.$axios
        .get(url)
        .then((response) => {
          this.PaymentSchema = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },

    GoNew() {
      this.getGetBoxClose5();
      this.getGetBoxClose6();
    },
    async getGetBoxClose5() {
      let url = `Transaction/GetBoxClose?startDate=${this.startDate}&endDate=${this.endDate}&TransationType=5`;
      this.FormId = this.$route.query.Form;
      this.$axios
        .get(url)
        .then((response) => {
          this.Box5 = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async getGetBoxClose6() {
      let url = `Transaction/GetBoxClose?startDate=${this.startDate}&endDate=${this.endDate}&TransationType=6`;
      this.FormId = this.$route.query.Form;
      this.$axios
        .get(url)
        .then((response) => {
          this.Box6 = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
  },
};
</script>

<style></style>

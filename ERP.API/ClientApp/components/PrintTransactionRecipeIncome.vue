<template>
  <div >
    <div class="card-body ">
      <div class="d-print-none mt-4 text-center">
        Vista de factura para imprimir
      </div>

      <div
        class="ticket"
        style="font: 14px Lucida Console; background-color: white; color: black"
      >
        <br />
        <div class="container">

            <div class="row  justify-content-between align-items-center mb-3">
              <img src="https://picsum.photos/200" style="width:100px; height:100px;" />
              <div class="col-md-3">
                <p>Caja: {{ PrincipalSchema.box }}</p>
                <p>Fecha: {{ FormatDate(PrincipalSchema.date) }}</p>
              </div>
            </div>

          <div class="row">
            <div class="col-md-8 ml-auto">
                <b-form-group label="Recibimos de" label-cols="3" class="mb-1 my-">

                  <b-form-input
                    class="ledger-input "
                    type="text"
                    size="sm"
                    v-model="PrincipalSchema.contactName"
                    disabled
                  ></b-form-input>
                </b-form-group>
            </div>
              <div class="row ml-0">
                <div class="col-lg-12">
                  <b-form-group>
                      <b-form-group label="Recibimos la cantidad de" label-cols="4" class="mb-2">
                        <b-form-input
                         class="ledger-input"
                          type="text"
                          size="sm"
                          disabled
                        ></b-form-input>
                      </b-form-group>
                  </b-form-group>
                </div>
              </div>
            </div>
            <div class="col-md-8">
              <label
              >
                Metodo Pago:
              </label>
              <label>
                {{ PrincipalSchema.paymentMethod }}
                <input type="radio" checked />
              </label>

            </div>
        </div>
        <hr>
        <table class="w-100">

          <tbody style="line-height: 1.6">
              <div class="row" v-for="(detail, index) in PrincipalSchema.transactionsDetails" :key="index">
                <div class="col-md-10 m-auto">
                  <b-form-group :label="detail.referenceId" label-cols="8" >
                    <b-form-input class="ledger-input w-75 text-center" size="sm" type="number" v-model.number="detail.price" disabled></b-form-input>
                  </b-form-group>
                </div>
              </div>
          </tbody>

        </table>

        <br /><span v-if="Ticket.invoiceComentary">
          {{ Ticket.invoiceComentary }}</span
        >
         <hr>
        <div class="row justify-content-end">
            <div class="col-md-5">
              <b-form-group label="Total" label-cols="2" class="mb-2 fs-5">
                <b-form-input class="ledger-input w-25 text-center" size="sm" type="text" v-model.number="PrincipalSchema.total" disabled></b-form-input>
              </b-form-group>
            </div>
        </div>

      </div>

      <div class="d-print-none mt-4">
        <div class="float-end">
          <a
            href="javascript:window.print()"
            class="btn btn-success waves-effect waves-light mr-1"
          >
            <i class="fa fa-print"></i> </a
          ><b-button variant="primary" v-if="Btn" class="btn" @click="GoNew()">
            <i class="mdi mdi-plus me-1"></i> Nuevo
          </b-button>
          <b-button
            variant="secundary"
            v-if="Btn"
            class="btn"
            @click="GoBack()"
          >
            <i class="bx bx-arrow-back"></i> Regresar
          </b-button>
        </div>
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
  props: ["Btn"],
  data() {
    return {
      PrincipalSchema:{
        contactName:"",
        currency:"",
        paymentMethod:"",
        bank:"",
        box:"",
        date:"",
        total:0,
        transactionsDetails:[]
      },
      FormId: "",
      file: null,
      DataForm: "",
      Ticket: { transactionReceipt:{}},
      PageCreate: "/ReciboIngreso",
      incomeReceipt: [
        { label: '10% Caja general', value:0 },
        { label: '1/3 Tercera parte final año', value: 0 },
        { label: '4% Intereses por atrasos', value: 0 },
        { label: 'Seguro Médico', value: 0 },
        { label: 'Seguro Vehículo', value: 0 },
        { label: 'Seguro Retiro', value: 0 },
        { label: 'Abono Capital', value: 0 },
        { label: 'Interés (4%)', value: 0 },
        { label: '10% Dólares', value: 0 },
        { label: '10% Euros', value: 0 },
        { label: 'Seguro Vejez Dólares', value: 0 },
        { label: 'Seguro Vejez Euros', value: 0 },
        { label: 'Abono deuda', value: 0 },
        { label: 'Hogar Madre Amadora', value: 0 },
        { label: 'Canonización del Cardenal Sancha', value: 0 },
        { label: 'Donaciones', value: 0 },
        { label: 'Formación', value: 0 },
        { label: 'Boletín Sanchino', value: 0 },
        { label: 'Telas, Hábito, Velas', value: 0 },
        { label: 'Libros', value: 0 },
      ]
    };
  },

  //middleware: "authentication",

  created() {
    this.FormId = this.$route.query.Form;
    this.GetForm();
    this.getTicket();
  },
  methods: {
    GetFile(SourceId) {
      this.$axios
        .get(`FileManager/GetBySourceIdFirst?SourceId=${SourceId}`)
        .then((response) => {
          this.file = response.data.data;

        })
        .catch((error) => {
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },

    GetForm() {
      var url = `Form/GetById?Id=${this.FormId}`;
      this.DataForm = {};
      this.$axios
        .get(url)
        .then((response) => {
          this.DataForm = response.data.data;
        })
        .catch((error) => {
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },

    GoBack() {
      this.$router.push({ path: `/ExpressForm/Index?Form=${this.FormId}` });
    },
    GoNew() {
      this.$router.push({
        path: `${this.PageCreate}`,
        query: {
          Form: this.FormId,
          Action: "create",
          id: null,
        },
      });
    },
    async getTicket() {
      try {

        let url = `TransactionReceipt/GetRecipeById?id=${this.$route.query.Id}`;
        this.FormId = this.$route.query.Form;
        const response = await this.$axios.get(url);
        this.Ticket = response.data.data;
        this.PrincipalSchema.currency = this.Ticket.transactionReceipt.currency.name
        this.PrincipalSchema.bank = this.Ticket.transactionReceipt.banks.name
        this.PrincipalSchema.contactName = this.Ticket.transactionReceipt.contact.name
        this.PrincipalSchema.paymentMethod = this.Ticket.transactionReceipt.paymentMethods.name
        this.PrincipalSchema.date = this.Ticket.transactionReceipt.date
        this.PrincipalSchema.transactionsDetails = this.Ticket.transactionReceiptDetails[0].transactions.transactionsDetails
        this.PrincipalSchema.total = this.Ticket.transactionReceiptDetails[0].transactions.globalTotal

          console.log(this.PrincipalSchema)
          console.log(this.Ticket.transactionReceipt)
          console.log(this)

          console.log(this.Ticket)
      } catch (error) {
        // this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        console.error(error);
      }

    },
  },
};
</script>

<style scoped>


.container{
  width: 90%;
  margin: auto;
}
.ledger-input {
  border: none;
  border-bottom: 1px solid rgb(156, 156, 156);
  border-radius: 0;
  background-color: transparent;
  box-shadow: none;
  color: black;
}
</style>

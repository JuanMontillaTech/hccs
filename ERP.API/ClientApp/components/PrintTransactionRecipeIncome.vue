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
        <div class="container-header">

          <div class="row  justify-content-between align-content-center mb-3">
            <div class="d-flex w-25 justify-content-between align-items-center">
              <img 
                             src="~/assets/images/logo-smsancha.png"
                             alt=""
                             style="width:100px; height:100px;"
                             class="logo logo-dark"
                        />
               <p class="w-50 m-0">
               HERMANAS DE LA CARIDAD DEL CARDENAL SANCHA
               </p> 
            </div>
            <div class="col-md-4 header-print">
              <p>Caja: {{ PrincipalSchema.box }}</p>
              <p>Fecha: {{ FormatDate(PrincipalSchema.date) }}</p>
            </div>
          </div>

        <div class="row">
          <div class="col-md-8 ml-auto mb-4 fs-5">
              <label >
                Recibimos de :
              </label>

                <span
                style="font: 18px Lucida Console; background-color: white; color: black"
                >
                {{ PrincipalSchema.contactName }}
              </span>
          </div>
            <div class="row ml-0">
              <div class="col-lg-12 mb-4 fs-5">
                    <label>
                      Recibimos la cantidad de :
                    </label> 
                    <span
                      style="font: 18px Lucida Console; background-color: white; color: black"
                      >
                      {{SetTotal(PrincipalSchema.total)}} Pesos
                    </span>
              </div>
            </div>
          </div>
          <div class="col-md-8 fs-5">
            <label
            >
              Metodo Pago:
            </label>
            <label>
              {{ PrincipalSchema.paymentMethod }}
            </label>

        </div>
      </div>
        <hr>
        <table class="w-100">

          <tbody style="line-height: 1.6">
              <div class="row" v-for="(detail, index) in incomeReceipt" :key="index">
                <div class="col-md-10 m-auto concept-print">
                  <b-form-group class="fs-5" :label="detail.label" label-cols="8" >
                    <b-form-input class="ledger-input w-50 fs-5 text-center" size="sm" type="number" v-model.number="detail.value" disabled></b-form-input>
                  </b-form-group>
                </div>
              </div>
          </tbody>

        </table>

        <br /><span v-if="Ticket.invoiceComentary">
          {{ Ticket.invoiceComentary }}</span
        >
         <hr>
        <div class="row justify-content-end m-4 pb-4">
            <div class="col-md-5 d-flex align-content-center footer-print">
              <label class="fs-5">Total</label>
              <span class="ledger-input w-50 fs-5 text-center">{{SetTotal(PrincipalSchema.total)}}</span>
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
        this.PrincipalSchema.box = this.Ticket.transactionReceiptDetails[0].transactions.box.name
          for (let transactionDetail of this.PrincipalSchema.transactionsDetails)
          {
              let url = `LedgerAccount/GetById?Id=${transactionDetail.referenceId}`;

              try {
                const response = await this.$axios.get(url);
                const items = response.data.data;
                this.incomeReceipt.push({ label: items.name, value:transactionDetail.price })
    
              } catch (error) {
                console.log(error)
              }
          }
          this.GetFile(this.Ticket.companyId);
    
      } catch (error) {
        // this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        console.error(error);
      }

    },
  },
};
</script>

<style scoped>
@media print{
  .container-header{
    width: 100%;
    margin: auto;
  }
  .header-print,.footer-print{
    width: 100%;
    display: flex;
    flex-direction: column;
    justify-content: end;
    align-items: end;
  }
  .footer-print{
    flex-direction: row; 
    width: 50%;
    margin-right: 50px;
  }

  .concept-print{
    width: 90%;
    margin: auto;
  }
}

.container-header{
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

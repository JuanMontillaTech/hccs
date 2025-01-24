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
          <table>
            <tr>
              <td>   <img
                src="~/assets/images/logo-smsancha.png"
                alt=""
                style="width:100px; height:100px;"
                class="logo logo-dark"
              /></td><th class="col-md-12 text-center">     <h3 class="m-0">HERMANAS DE LA CARIDAD DEL CARDENAL SANCHA</h3></th>
              <td  style="font-size: medium; border: 0px !important;">
                <p class="m-0"> {{ Ticket.transactionReceipt.document }}</p>
                <p class="m-0">Fecha: {{ FormatDate(PrincipalSchema.date) }}</p>
              </td>
            </tr>
          </table>


        </div>
        <hr>
        <div class="container-header">

          <table    style="margin-left: 10px; font-size: medium; border: 0px !important;">
            <tr>
              <td>Recibimos de: {{ PrincipalSchema.contactName }}</td>
            </tr>
            <tr>
              <td>Forma: {{  PrincipalSchema.paymentMethod  }}</td>
            </tr>
          </table>

          <hr>

        <table class="w-100  "  style="margin-left: 10px; font-size: medium;">
          <thead>
          <tr>
            <th class="text-left">Descripción</th>
            <th class="text-left">Cantidad</th>
          </tr>
          </thead>
          <tbody style="line-height: 0.5;">

          <tr v-for="(detail, index) in orderedIncomeReceipt" :key="index" v-if="detail.value !== 0">
            <td>
              <label>{{ detail.label }}</label>
            </td>
            <td class="text-left">
              ${{ SetTotal(detail.value) }}
            </td>
          </tr>



          </tbody>
          <tfoot  >

          <tr>
            <th style="text-align: right;"> Total:</th>
            <td> ${{SetTotal(PrincipalSchema.total)}}</td>
          </tr>
          </tfoot>
        </table>
        <hr>
        {{ PrincipalSchema.commentary }}
      </div>


        <br /><span v-if="Ticket.invoiceComentary">
          {{ Ticket.invoiceComentary }}</span
      >
        <hr>
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
        Document:"None",
        contactName:"",
        currency:"",
        paymentMethod:"",
        bank:"",
        box:"",
        date:"",
        total:0.0,
        transactionsDetails:[],
        transactionExt:[],
        commentary:""

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

  computed: {
    orderedIncomeReceipt() {
      return [...this.incomeReceipt].sort((a, b) => a.index - b.index);
    }
  },
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



        let url = `TransactionReceipt/GetRecipeByIdForPrint?id=${this.$route.query.Id}`;
        this.FormId = this.$route.query.Form;
        const response = await this.$axios.get(url);
        this.Ticket = response.data.data;
        this.PrincipalSchema.currency = this.Ticket.transactionReceipt.currency.name
        this.PrincipalSchema.bank = this.Ticket.transactionReceipt.box.name
        this.PrincipalSchema.contactName = this.Ticket.transactionReceipt.contact.name
        this.PrincipalSchema.paymentMethod = this.Ticket.transactionReceipt.paymentMethods.name
        this.PrincipalSchema.date = this.Ticket.transactionReceipt.date
        this.PrincipalSchema.total = this.Ticket.transactionReceipt.total
        this.PrincipalSchema.transactionsDetails = this.Ticket.transactionReceipt.transactionReceiptDetails
        this.PrincipalSchema.transactionExt = this.Ticket.transactionExt
        this.PrincipalSchema.commentary = this.Ticket.transactionReceipt.commentary


        this.PrincipalSchema.box = this.Ticket.transactionReceipt.box.name
          for (let transactionDetail of this.PrincipalSchema.transactionExt)
          {

              try {


                this.incomeReceipt.push({ label: transactionDetail.label, value:transactionDetail.value ,index: transactionDetail.index })

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
table {
  border-collapse: collapse;
}
td, th {
  border: none;
}
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

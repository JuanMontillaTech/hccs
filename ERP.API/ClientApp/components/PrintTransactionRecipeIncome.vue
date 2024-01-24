<template>
  <div>
    <div class="card-body">
      <div class="d-print-none mt-4 text-center">
        Vista de factura para imprimir
      </div>

      <div
        class="ticket"
        style="font: 14px Lucida Console; background-color: white; color: black"
      >
        <img v-if="file" :src="file.link" class="w-25 h-25" />

        <table class="w-100">
          <thead>
            <tr v-if="Ticket.companyName">
              <td>{{ Ticket.companyName }}</td>
            </tr>
            <tr v-if="Ticket.companyPhones">
              <td>{{ Ticket.companyPhones }}</td>
            </tr>
            <tr v-if="Ticket.taxId">
              <td>RNC: {{ Ticket.taxId }}</td>
            </tr>
            <tr v-if="Ticket.taxNumber">
              <td>Comprobante: {{ Ticket.taxNumber }}  </td>
            </tr>

            <tr>
              <td>#{{ Ticket.invoiceCode }}</td>
              <td>Fecha: {{ FormatDate(Ticket.date) }}</td>
            </tr>
            <tr v-if="Ticket.invoiceContactName">
              <td>Cliente: {{ Ticket.invoiceContactName }}</td>
              <td v-if="Ticket.invoiceContactPhone">
                Tel.: {{ Ticket.invoiceContactPhone }}
              </td>
            </tr>
            <tr v-if="Ticket.taxContactNumber">
              <td>RNC Cliente: {{ Ticket.taxContactNumber }}</td>
            </tr>
            <tr v-if="Ticket.invoiceContactAdress">
              <td>Direccion: {{ Ticket.invoiceContactAdress }}</td>
            </tr>
            <tr v-if="Ticket.invoicePaymentMethodId">
              <td>Pago con: {{ Ticket.invoicePaymentMethod }}</td>
            </tr>
            <tr v-if="Ticket.invoicePaymentTermId">
              <td>Forma de pago: {{ Ticket.invoicePaymentTerm }}</td>
            </tr>
          </thead>
        </table>
        <br />
            <div class="row justify-content-end ">
              <div class="col-md-2">
                <b-form-group
                  label="Caja"
                  class="mb-2"
                >
                    <!-- :options="ListBox"
                    :reduce="(row) => row.id"
                    label="name"
                    v-model="principalSchema.boxId"
                    size="sm" -->
                  <vueselect
                  >
                  </vueselect>
                </b-form-group>

              </div>
            </div>
            <div class="col-md-6 ml-auto">
                <b-form-group label="Recibimos de" label-cols="3" class="mb-2">
                  <!-- <vSelectContact
                    v-if="item"
                    :field="item"
                    size="sm"
                    :select="Scheme[item.field]"
                    @CustomChange="GetLitValue"
                  >
                  </vSelectContact> -->
                </b-form-group>
              </div>
            <div class="row">
              <div class="row ml-0 mb-3">
                <div class="col-lg-5">
                  <b-form-group>
                      <b-form-group label="Recibimos la cantidad de" label-cols="5" class="mb-2">
                        <b-form-input
                          type="number"
                          size="sm"
                        ></b-form-input>
                      </b-form-group>
                  </b-form-group>
                </div>
              </div>
            </div>
        <table class="w-100">

          <tbody style="line-height: 1.6">
              <div class="row" v-for="(ledger, index) in incomeReceipt" :key="index">
                <div class="col-md-10 m-auto">
                  <b-form-group :label="ledger.label" label-cols="8" class="mb-2">
                    <b-form-input class="ledger-input w-75 text-center" size="sm" type="number" v-model.number="ledger.value" onchange="calcularTotal"></b-form-input>
                  </b-form-group>
                </div>
              </div>
          </tbody>
          <tfoot v-if="!Ticket.taxNumber">

            <tr>
              <td></td>
              <td></td>
              <td class="text-right" >Sub-Total</td>
              <td
                   style="  text-decoration: overline;  text-decoration-thickness: auto;  "
              >
                ${{ SetTotal (Ticket.totalAmount  )}}

              </td>

            </tr>
            <tr  >
              <td>  </td>
              <td></td>
              <td class="text-right"  >ITBIS</td>
              <td

              >

                ${{ SetTotal (0) }}
              </td>
            </tr>
            <tr >
              <td></td>
              <td></td>
              <td class="text-right">Total</td>
              <td>
                <span
                  style=" text-decoration: overline;  text-decoration-thickness: auto;  "
                >
                  ${{  SetTotal (Ticket.invoiceTotal )}}
                </span>
              </td>
            </tr>

          </tfoot>

          <tfoot v-if="Ticket.taxNumber">

          <tr>
            <td></td>
            <td></td>
            <td class="text-right" >Sub-Total</td>

            <td
                style="  text-decoration: overline;  text-decoration-thickness: auto;  "
            >
              ${{ SetTotal (Ticket.totalAmountTax  )}}

            </td>
          </tr>
          <tr   >
            <td>  </td>
            <td></td>
            <td class="text-right"  >ITBIS</td>
            <td

            >

              ${{ SetTotal (Ticket.invoiceTotalTax) }}
            </td>
          </tr>

          <tr>
            <td></td>
            <td></td>
            <td  class="text-right">Total</td>
            <td>
                <span
                  style=" text-decoration: overline;  text-decoration-thickness: auto;  "
                >

                  ${{  SetTotal (Ticket.globalTotalTax )}}
                </span>
            </td>
          </tr>
          </tfoot>

        </table>

        <br /><span v-if="Ticket.invoiceComentary">
          {{ Ticket.invoiceComentary }}</span
        >
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
      FormId: "",
      file: null,
      DataForm: "",
      Ticket: { transactionReceipt:{}},
      PageCreate: "/ReciboIngreso",
      incomeReceipt: [
        { label: '10% Caja general', value:0 },
        { label: '1/3 Tercera parte final año', value: 0 },
        { label: '4% Intereses por atrasos', value: 0 },
        { label: 'Seguro Médico', value: 12 },
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
        { label: 'Libros', value: '' },
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
      let url = `TransactionReceipt/GetRecipeById?id=${this.$route.query.Id}`;
      this.FormId = this.$route.query.Form;
      this.$axios
        .get(url)
        .then((response) => {
          this.Ticket = response.data.data;
          console.log(this.Ticket)
        })
        .catch((error) => {
        //  this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
  },
};
</script>

<style></style>

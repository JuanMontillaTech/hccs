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
        <table class="w-100">
          <thead>
            <tr>
              <th class="text-left">CANT.</th>
              <th class="text-left">DESCRIP.</th>
              <th class="text-right">PRECIO</th>
              <th class="text-right">VALOR</th>
            </tr>
          </thead>
          <tbody style="line-height: 1.6">
            <tr v-for="(item, index) in Ticket.ticketDetallisDtos" :key="index">
              <td class="quantity width:10%">{{ item.amount }}</td>
              <td class="description width:70%">
                {{ item.reference }} {{ item.description }}
              </td>
              <td class="price width:10%" v-if="!Ticket.taxNumber">${{  item.price  }}</td>
              <td class="price width:10%"  v-if="!Ticket.taxNumber">${{ item.total }}</td>
              <td class="price width:10%"  v-if="Ticket.taxNumber">${{  item.priceWithTax  }}</td>
              <td class="price width:10%"  v-if="Ticket.taxNumber">${{ item.totalTax }}</td>
            </tr>
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
      PageCreate: "/ExpressForm/FuncionalFormExpress",
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
        })
        .catch((error) => {
        //  this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
  },
};
</script>

<style></style>

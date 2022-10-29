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
        {{ Ticket.companyName }}

          <table class="w-100">
            <thead>
                
              <tr>
                <td>{{ Ticket.companyPhones }}</td>
              </tr>
              <tr>
                <td>Factura: {{ Ticket.invoiceCode }}</td>
                <td>Fecha: {{ FormatDate(Ticket.date) }}</td>
              </tr>
              <tr>
                <td>Cliente: {{ Ticket.invoiceContactName }}</td>
                <td v-if="Ticket.invoiceContactPhone">
                  Tel.: {{ Ticket.invoiceContactPhone }}  
                </td>
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
        <br>
          <table class="w-100">
            <thead>
              <tr>
                <th class="text-left">CANT.</th>
                <th class="text-left">DESCRIP.</th>
                <th class="text-right">PRECIO</th>
                <th class="text-right">VALOR</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, index) in Ticket.ticketDetallisDtos" :key="index">
                <td class="quantity width:10%">{{ item.amount }}</td>
                <td class="description width:70%">
                  {{  item.reference }} {{  item.description }}
                </td>
                <td class="price width:10%">${{ item.price }}</td>
                <td class="price width:10%">${{ item.total }}</td>
              </tr>
            </tbody>
            <tbody>
              <tr>
                <td></td>
                <td></td>
                <td class="text-right">Total</td>
                <td class="text-right">  {{ Ticket.invoiceTotal }}</td> 
              </tr>
            </tbody>
          </table>
     
          <br /><span v-if="Ticket.invoiceComentary"
            >Comentario: {{ Ticket.invoiceComentary }}</span
          >
        
        </div>
        <div class="d-print-none mt-4">
          <div class="float-end">
            <a
              href="javascript:window.print()"
              class="btn btn-success waves-effect waves-light mr-1"
            >
              <i class="fa fa-print"></i>
            </a><b-button variant="primary" class="btn" @click="GoNew()">
                    <i class="mdi mdi-plus me-1"></i> Nuevo
                          </b-button>
            <b-button variant="secundary" class="btn" @click="GoBack()">
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
  data() {
    return {
      FormId: "",
      Ticket:[],
      PageCreate:"/ExpressForm/FuncionalFormExpress",
       
    };
  },

  //middleware: "authentication",

  created() {
    this.getTicket();
   
    
  },
  methods: {
    
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },
 
     
    GoBack() {
      this.$router.push({ path: `/ExpressForm/Index?Form=${this.FormId}` });
    },
    GoNew(){
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
      let url = `Transaction/GetTicket?id=${this.$route.query.Id}`;
      this.FormId = this.$route.query.Form;
      this.$axios
        .get(url)
        .then((response) => {
          this.Ticket = response.data.data;
        
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    
  } 
  },
};
</script>

<style>
 


</style>

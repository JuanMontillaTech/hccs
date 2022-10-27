<template>
  <div>
    <div>
      <div class="card-body">
        <div class="d-print-none mt-4 text-center">
          Vista de factura para imprimir
        </div>
        <div
          class="ticket"
          style="font: 10px Lucida Console; background-color: white"
         
        >
        {{ company.companyName }}
          <table>
            <thead>
               
              
              <tr>
                <td>{{ company.address }}</td>
              </tr>
              <tr>
                <td>{{ company.phones }}</td>
              </tr>
              <tr>
                <td>Factura: {{ principalSchema.code }}</td>
                <td>Fecha: {{ FormatDate(principalSchema.date) }}</td>
              </tr>
              <tr>
                <td>Cliente: {{ Contact.name }}</td>
                <td>
                  Tel.: {{ Contact.cellPhone }} {{ Contact.phone1 }}
                  {{ Contact.phone1 }}
                </td>
              </tr>
              <tr v-if="Contact.address">
                <td>Direccion: {{ Contact.address }}</td>
              </tr>
              <tr>
                <td>Forma de pago: {{ principalSchema.PaymentMethod }}</td>
              </tr>
            </thead>
          </table>

          <table>
            <thead>
              <tr>
                <th class="text-left">CANT.</th>
                <th class="text-left">DESCRIP.</th>
                <th class="text-right">PRECIO</th>
                <th class="text-right">VALOR</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, index) in list" :key="index">
                <td class="quantity width:10%">{{ item.amount }}</td>
                <td class="description width:70%">
                  {{ GetConcept(item.referenceId) }}
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
                <td class="text-right">  {{ principalSchema.globalTotal }}</td> 
              </tr>
            </tbody>
          </table>
     
          <br /><span v-if="principalSchema.commentary"
            >Comentario: {{ principalSchema.commentary }}</span
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
    </div>
 
</template>

<script>
var numbro = require("numbro");
var moment = require("moment");
export default {
  head() {
    return {
      title: `${this.DataForm.title} | ERP`,
    };
  },
  data() {
    return {
      FormId: "",
      PageCreate:"/ExpressForm/FuncionalFormExpress",
      company:[],
      DataForm: [],
      DataFormSection: [],
      fields: [],
      principalSchema: {
        contactId: null,
        code: null,
        date: null,
        reference: null,
        PaymentMethod: "",
        paymentMethodId: null,
        globalDiscount: 0.0,
        globalTotal: 0.0,
        transactionsType: 1,
        transactionsDetails: null,
        numerationId: null,
        commentary: "",
      },
      schema: {
        id: null,
        transactionsId: null,
        referenceId: null,
        description: null,
        amount: 1,
        price: 0,
        discount: 0,
        total: 0,
        tax: 0,
      },
      conceptSelectList: [],
      Contact: [],
      rows: [],
      invoice_subtotal: 0,
      invoice_total: 0,
      invoice_tax: 0,
      list: [
        {
          id: null,
          concept: null,
          transactionsId: null,
          referenceId: null,
          description: null,
          amount: 1,
          price: 0,
          discount: 0,
          total: 0,
          tax: 0,
        },
      ],
    };
  },

  //middleware: "authentication",

  created() {
 
      this.getTransactionsDetails();
    
  },
  methods: {
    getCompany() {
      let url =  `Company/GetDefault`;

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
 
    GetLitValue(filds, Value) {
      this.principalSchema[filds] = Value;
    },
   
    
    GetCustomer() {
      let url = `Contact/GetById?id=${this.principalSchema.contactId}`;

      this.$axios
        .get(url)
        .then((response) => {
          this.Contact = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetConcept(referenceId) {
      let _element = "Articulo si existe";
      if (referenceId != null) {
        this.conceptSelectList.forEach((element) => {
          if (element.id == referenceId) {
            console.log(element.reference);
            _element = element.reference;
          }
        });
        return _element;
      } else {
        return "Articulo no existe";
      }
    },
    GetCurrency(PaymentMethodId) {
      let url = `PaymentMethod/GetById?id=${PaymentMethodId}`;

      this.$axios
        .get(url)
        .then((response) => {
          this.principalSchema.PaymentMethod = response.data.data.name;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetProduct() {
      this.$axios
        .get(`Concept/GetAll`)
        .then((response) => {
          this.conceptSelectList = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
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
    async getTransactionsDetails() {
      let url = `Transaction/GetById?id=${this.$route.query.Id}`;
      this.FormId = this.$route.query.Form;
      this.$axios
        .get(url)
        .then((response) => {
          this.principalSchema = response.data.data;
          this.list = response.data.data.transactionsDetails;
          this.GetCurrency(this.principalSchema.paymentMethodId);
          this.GetCustomer();
          this.GetProduct();
          this.getCompany();
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
  },
};
</script>

<style></style>

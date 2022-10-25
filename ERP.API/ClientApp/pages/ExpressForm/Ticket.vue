<template>
  <div  >
    <div class="card ">
                <div class="card-body">

                  <div class="ticket" style="font: 10px Lucida Console;" >
                         
                         <p class="centered">RECEIPT EXAMPLE
                             <br>Address line 1
                             <br>Address line 2</p>
                         <table>
                             <thead>
                                 <tr>
                                     <th class="quantity">#</th>
                                     <th class="description">Descrición</th>
                                     <th class="price">Precio</th>
                                     <th class="price">Total</th>
                                 </tr>
                             </thead>
                             <tbody>
                                 <tr>
                                     <td class="quantity">1.00</td>
                                     <td class="description">ARDUINO UNO R3</td>
                                     <td class="price">$25.00</td>
                                     <td class="price">$25.00</td>
                                 </tr>
                                 <tr>
                                     <td class="quantity">2.00</td>
                                     <td class="description">JAVASCRIPT BOOK</td>
                                     <td class="price">$10.00</td>
                                     <td class="price">$20.00</td>
                                 </tr>
                                 <tr>
                                     <td class="quantity">1.00</td>
                                     <td class="description">STICKER PACK</td>
                                     <td class="price">$10.00</td>
                                 </tr>
                                 <tr>
                                     <td class="quantity"></td>
                                     <td class="description">TOTAL</td>
                                     <td class="price">$55.00</td>
                                     
                                 </tr>
                             </tbody>
                         </table>
                        {{DataForm}}
                     </div>
                     <div class="d-print-none mt-4">
                                          <div class="float-end">
                                              <a href="javascript:window.print()" class="btn btn-success waves-effect waves-light mr-1">
                                                  <i class="fa fa-print"></i>
                                              </a>
                                             
                                          </div>
                                      </div>
                </div>
    </div>
      
  </div>
</template>

<script>
export default {
  head() {
    return {
      title: `${this.DataForm.title} | ERP`,
    };
  },
  data() {
    return {
      FormId: "",
      DataForm: [],
      DataFormSection: [],
      fields: [],
      principalSchema: {
        contactId: null,
        code: null,
        date: null,
        reference: null,
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
    this.FormId = this.$route.query.Form;
    this.GetFormRows();
    if (this.$route.query.Action === "print") {
      this.getTransactionsDetails();
    }
  },
  methods: {
    
    GetValueFormElement(formElemen) {
      this.principalSchema = formElemen;
    },
    
    GetLitValue(filds, Value) {
      this.principalSchema[filds] = Value;
    },
    GetFilterDataOnlyshowForm(fields) {
      let results = fields.filter((rows) => rows.showForm == 1);
      return results;
    },
    setSelected(data, idx) {
      var obj = this.list.find((element, index) => index === idx);
      let row = this.conceptSelectList.find(
        (element) => element.id == obj.referenceId
      );
      obj.referenceId = row.id;
      obj.price = row.priceSale;
      this.calculateLineTotal(obj);
    },
    GetFormRows() {
      var url = `Form/GetById?Id=${this.FormId}`;
      this.DataForm = [];
      this.DataFormSection = [];
      this.$axios
        .get(url)
        .then((response) => {
          this.DataForm = response.data.data;

        
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetFilds() {
      this.$axios
        .get(`Formfields/GetSectionWithFildsByFormID/${this.FormId}`)
        .then((response) => {
          this.DataFormSection = response.data.data;
          this.GetProduct();
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
    // calculateTotal() {
    //   var subtotal, total;
    //   subtotal = this.list.reduce(function (sum, product) {
    //     var lineTotal = parseFloat(product.total);
    //     if (!isNaN(lineTotal)) {
    //       return sum + lineTotal;
    //     }
    //   }, 0);

    //   this.invoice_subtotal = subtotal.toFixed(2);

    //   total = subtotal * (this.invoice_tax / 100) + subtotal;
    //   total = parseFloat(total);
    //   if (!isNaN(total)) {
    //     this.invoice_total = total.toFixed(2);
    //     this.principalSchema.globalTotal = this.invoice_total;
    //   } else {
    //     this.invoice_total = "0.00";
    //     this.principalSchema.globalTotal = this.invoice_total;
    //   }
    // },
    // calculateLineTotal(invoiceProduct) {
    //   var total =
    //     parseFloat(invoiceProduct.price) * parseFloat(invoiceProduct.amount);
    //   if (!isNaN(total)) {
    //     invoiceProduct.total = total.toFixed(2);
    //   }
    //   this.calculateTotal();
    // },
  
    GoBack() {
       
        this.$router.push({ path: `/ExpressForm/Index?Form=${this.FormId}` });
      
    },

 
  
    async getTransactionsDetails() {
      let url = `Transaction/GetById?id=${this.$route.query.Id}`;
      console.log(this.$route.query.Id);
      // this.$axios
      //   .get(url)
      //   .then((response) => {
      //     this.principalSchema = response.data.data;
      //     this.list = response.data.data.transactionsDetails;
      //     this.calculateTotal();
      //   })
      //   .catch((error) => {
      //     this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
      //   });
    }, 
  },
};
</script>

<style>

</style>
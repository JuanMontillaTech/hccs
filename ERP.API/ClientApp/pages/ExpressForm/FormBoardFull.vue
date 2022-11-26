<script>
import Swal from "sweetalert2";

var moment = require("moment");
var numbro = require("numbro");
/**
 * User grid component
 */
export default {
  head() {
    return {
      title: ` `,
    };
  },
  layout: "PosLayoust",
  data() {
    return {
      title: "Mesas",
      FormId:"25f94e8c-8ea0-4ee0-adf5-02149a0e080b",
      Spinning: true,
      PaymentMethod: [],
      PaymentMethodSelect: null,
      MytableData: [],
      Tablelistselect: {},
      Tablelist: [],
      tableData: [],
      tableDataOrder: [],
      ListInvoiceOrder: [],
      invoice_total: 0,
      GlobalTotalInvocie: 0,
      Payinvoice: 0,
      PayChangeinvoice: 0,
      IsPay: false,
      items: [
        {
          text: "",
        },
        {
          text: "Pago de Ordenes",
          active: true,
        },
      ],
    };
  },
  watch: {
    Tablelistselect: function (val) {
      this.GetByTransactionLocation();
    },
  },
  mounted() {
    this.GetTablelist();
    this.GetPaymentMethod();
  },
  middleware: "authentication",

  methods: {
    ValidePay() {
      var InvoiceSys = parseFloat(this.GlobalTotalInvocie);
      var PayInvoiceSys = parseFloat(this.Payinvoice);      
      if (InvoiceSys <= PayInvoiceSys) {
        this.IsPay = true;
        this.PayChangeinvoice =this.SetTotal( PayInvoiceSys - InvoiceSys);
      } else {
        this.PayChangeinvoice = 0;
        this.IsPay = false;
        this.$toast.info(
          `Monto a facturar el menor al efectivo`,
          "Pago no es suficiente",
          this.izitoastConfig
        );
      }
      if (this.PaymentMethodSelect != null) {
        this.IsPay = true;
      } else {
        this.IsPay = false;
        this.$toast.info(
          `Seleccione un metodo de pago`,
          "Seleccione un metodo de pago",
          this.izitoastConfig
        );
      }
      if (InvoiceSys > 0) {
        this.IsPay = true;
      } else {
        this.IsPay = false;
        this.$toast.info(
          `No tiene Total a pagar`,
          "No tiene Total a pagar",
          this.izitoastConfig
        );
      }
    },
    printForm(id) {
   
   this.$router.push({
     path: `/ExpressForm/posPrint?Action=print&Form=${this.FormId}&Id=${id}`,
   });
 },
    Pay() {
      this.ValidePay();
      let url = `Transaction/ProccesLocation/${this.Tablelistselect.id}/${this.PaymentMethodSelect.id}`;
      let result = null;
      console.log("dntro",url)

      this.$axios
        .post(url)
        .then((response) => {
          result = response;
          this.$toast.success(
            "Pagado.",
            "ÉXITO"
          );
          
          this.GetTable(this.Tablelistselect);
          this.printForm(result.data.data.id);
           
        })
        .catch((error) => {
          result = error;
          mixpanel.track("Pagos pos/Post" + result);
          this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
        });
    },

    GetPaymentMethod() {
      this.$axios
        .get(`PaymentMethod/GetAll`)
        .then((response) => {
          this.PaymentMethod = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetTable(itemsDetails) {
      this.Tablelistselect = itemsDetails;
      let url = `TransactionLocationTransaction/GetByTransactionLocationId?Id=${itemsDetails.id}`;

      this.Spinning = true;
      this.$axios
        .get(url)
        .then((response) => {
          this.Spinning = false;
          this.tableDataOrder = [];
          this.tableDataOrder = response.data.data;         
          this.calculateTotal();
          return response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    calculateTotal() {
      var subtotal, total;
      this.Payinvoice = 0;
      this.PayChangeinvoice = 0;
   
      subtotal = this.tableDataOrder.reduce(function (sum, product) {
        var lineTotal = parseFloat(product.transactions.globalTotal);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);
      
      this.GlobalTotalInvocie = subtotal;
      this.invoice_total = this.SetTotal(subtotal);
    },
    GetTablelist() {
      this.Spinning = true;
      this.$axios
        .get(`TransactionLocation/GetAll`)
        .then((response) => {
          this.Spinning = false;
          this.Tablelist = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },
    confirmCancellation(id) {
      let url =`Transaction/TransactionssDetailsDelete/${id}`;
      
     
      Swal.fire({
        title: "estas seguro?",
        text: "esta seguro que quiere remover Articulo",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si , Remuévela!",
        cancelButtonText: 'Cancelar',
      }).then((result) => {
        if (result.isConfirmed) {
          this.$axios
            .delete(url)
            .then((response) => {
              Swal.fire("Removido!", "El registro esta removido.", "success");
              this.GetTable(this.Tablelistselect);
            })
            .catch((error) => alert(error));
        }
      });
    },
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    async GetByTransactionLocation() {
      let url = `TransactionLocationTransaction/GetByTransactionLocationId?Id=${this.Tablelistselect.id}`;

      this.Spinning = true;
      this.$axios
        .get(url)
        .then((response) => {
          this.Spinning = false;
          this.tableDataOrder = [];
          this.tableDataOrder = response.data.data;
          return response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
  },
};
</script>

<template>
  <div>
    <PageHeader :title="title" :items="items" />
    <div class="card">
      <div class="card-title">
        <div class="d-flex flex-wrap w-100">
          <div class="mb-auto p-2" v-for="item in Tablelist" :key="item.id">
            <button @click="GetTable(item)" class="btn btn-primary">
              {{ item.name }}
            </button>
          </div>
        </div>
      </div>

      <div class="card-body">
        <b-alert show
          >{{ tableDataOrder.length }} Ordenes de Mesa
          {{ Tablelistselect.name }}
        </b-alert>

        <div class="text-center" v-if="Spinning">
          <b-spinner variant="success" label="Spinning"></b-spinner>
        </div>
        <div class="row">
          <div class="col-4">
            <table class="w-10" v-for="item in tableDataOrder" :key="item.id">
              <thead v-if="item.isActive">
                <tr>
                  <th>
                    <b-alert variant="success" show>
                      {{ item.transactions.code }}
                    </b-alert>
                  </th>
                </tr>
              </thead>
              <tbody v-if="item.isActive">
                <tr>
                  <td>
                    <b-list-group
                      v-for="itemsDetails in item.transactions
                        .transactionsDetails"
                      :key="itemsDetails.id"
                    >
                      <b-list-group-item variant="info" v-if="itemsDetails.isActive">
                        <b-button size="sm" variant="danger" @click="confirmCancellation(itemsDetails.id)">
                          <i class="fas fa-trash"></i>
                        </b-button>
                        {{ itemsDetails.amount }} - ${{
                          SetTotal(itemsDetails.price)
                        }}

                        - {{ itemsDetails.concept.description }}
                      </b-list-group-item>
                    </b-list-group>
                  </td>

                  <td></td>
                </tr>
              </tbody>
              <tfoot class="text-center" v-if="item.isActive">
                <tr>
                  <th>
                    <b-alert variant="warning" show>
                      Total de la orden ${{
                        SetTotal(item.transactions.globalTotal)
                      }}
                    </b-alert>
                  </th>
                </tr>
              </tfoot>
            </table>
          </div>
          <div class="col-8">
            <div class="row">
              <div class="col-4">
                Metodo de pago
                <select v-model="PaymentMethodSelect" class="col-form-label">
                  <option
                    class="dropdown-item"
                    v-for="item in PaymentMethod"
                    :key="item.id"
                    :value="item"
                  >
                    <h4>{{ item.name }}</h4>
                  </option>
                </select>
              </div>
            </div>
            <div class="row">
              <div class="col-4">
                <b-form-group>
                  <h4 class="card-title">Total a pagar:</h4>
                  <b-form-input v-model="invoice_total" disabled></b-form-input>
                </b-form-group>
              </div>
            </div>
            <div class="row">
              <div class="col-4">
                <b-form-group>
                  <h4 class="card-title">Efectivo:</h4>
                  <b-form-input
                    v-model="Payinvoice"
                   
                    @change="ValidePay()"
                  ></b-form-input>
                </b-form-group>
              </div>
            </div>
            <div class="row">
              <div class="col-4">
                <b-form-group>
                  <h4 class="card-title">Devuelta:</h4>
                  <b-form-input
                    v-model="PayChangeinvoice"
                    disabled
                  ></b-form-input>
                </b-form-group>
              </div>
            </div>
            <div class="row">
              <div class="col-4">
                <b-button
                  variant="success"
                  v-if="IsPay"
                  @click="Pay()"
                  class="btn"
                >
                  <i class="bx bx-save"></i> Pagar
                </b-button>
                <b-button
                  variant="danger"
                  v-if="IsPay == false"
                  @click="ValidePay()"
                  class="btn"
                >
                  <i class="bx bx-save"></i> Pagar
                </b-button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

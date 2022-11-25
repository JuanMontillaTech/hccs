<script>
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
      Spinning: true,
      invoice_total:0,
      PaymentMethod: [],
      PaymentMethodSelect: null,
      MytableData: [],
      Tablelistselect: {},
      Tablelist: [],
      tableData: [],
      tableDataOrder: [],
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
          return response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
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
              <thead>
                <tr>
                  <th>
                    <b-alert variant="success" show>
                      {{ item.transactions.code }}
                    </b-alert>
                  </th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>
                    <b-list-group
                      v-for="itemsDetails in item.transactions
                        .transactionsDetails"
                      :key="itemsDetails.id"
                    >
                      <b-list-group-item variant="info">
                        <b-button size="sm" variant="danger">
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
              <tfoot class="text-center">
                <tr>
                  <th>
                    <b-alert variant="warning" show>
                      Total {{ item.transactions.transactionsDetails.length }}
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
                  <h4 class="card-title"> Total a pagar:</h4>
                  <b-form-input v-model="invoice_total" disabled></b-form-input>
                </b-form-group>
               
              </div>
            </div>
            <div class="row">
              <div class="col-4">
                
                <b-button variant="success" class="btn">
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

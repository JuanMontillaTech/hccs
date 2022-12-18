<script>
var numbro = require("numbro");
var moment = require("moment");
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
      title: "COMANDAS",
      tableData: [],
      AllMenu: true,
      PaymentMethod: [],
      ElementConcept: [],
      PaymentMethodSelect: null,
      items: [
        {
          text: "COMANDAS",
        },
        {
          text: "COMANDAS",
          active: true,
        },
      ],
    };
  },
  middleware: "authentication",
  mounted() {
    this.GetPaymentMethod();
    this.myProvider();
    //this.getComan();
  },
  methods: {
    getComan() {
      var timesRun = 0;
     var interval = window.setTimeout(() => {
        timesRun += 1;
        if (timesRun === 60) {
          clearInterval(interval);
        }
        this.myProvider();
      }, 1000);
    

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
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },
    SendPayData(Transactions) {
      if (this.PaymentMethodSelect != null) {
        let Id_Send = Transactions.id;
        let url = `Transaction/Create`;
        let result = null;
        Transactions.id = null;
        Transactions.TransactionsType = 6;
        Transactions.FormId = "25f94e8c-8ea0-4ee0-adf5-02149a0e080b";
        Transactions.PaymentMethodId = this.PaymentMethodSelect;
        Transactions.Commentary = Transactions.code;
        this.$axios
          .post(url, Transactions)
          .then((response) => {
            result = response;
            this.myChangeStatus(
              Id_Send,
              "85685D53-D6A6-4381-944B-965ED1187FBD"
            );
            this.myProviderAll();
            this.printForm(response.data.data.id);
            this.$toast.success(
              "El Registro ha sido creado correctamente.",
              "ÉXITO"
            );
          })
          .catch((error) => {
            result = error;
            this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
          });
      } else {
        this.$toast.error("Seleccione un metodo de pago.", "Aviso");
      }
    },
    printForm(id) {
      this.$router.push({
        path: `/ExpressForm/Ticket?Action=print&Form=${this.FormId}&Id=${id}`,
      });
    },
    async myProviderAll() {
      let url = `Transaction/GetAllByTypeStatus?TransactionsTypeId=${8}`;
      this.AllMenu = true;
      this.$axios
        .get(url)
        .then((response) => {
          this.tableData = [];
          this.tableData = response.data.data;
          return response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetElementConcept(id) {
      var url = `TransactionsDetailsElement/GetByDetaillsId?Id=${id}`;
      this.ElementConcept = [];

      this.$axios
        .get(url)
        .then((response) => {
          this.ElementConcept = response.data.data;
        
           
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async myProvider() {
      let url = `Transaction/GetAllByTypeStatusIsService?TransactionsTypeId=${8}&TransactionStatusId=85685D53-D6A6-4381-944B-995ED1187FBA`;
      this.AllMenu = false;
      this.$axios
        .get(url)
        .then((response) => {
        
          this.tableData = [];
          this.tableData = response.data.data;
          return response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async myChangeStatus(Id, TransactionStatusId) {
      let url = `Transaction/SetStatus?TransactionId=${Id}&TransactionStatusId=${TransactionStatusId}`;

      this.$axios
        .get(url)
        .then((response) => {
          this.myProvider();
          return response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
  },
};
</script>

<template>
  <div>
    <PageHeader :title="title" :items="items" />
    <!-- <b-button-group class="mt-4 mt-md-0">
                <b-button size="sm" variant="info" @click="myProviderAll()">
                  Totas las ordenes
                </b-button>
              </b-button-group>
    <b-button-group class="mt-4 mt-md-0">
      <b-button size="sm" variant="info" @click="myProvider()">
        Comandas
      </b-button>
    </b-button-group> -->
    <div class="row">
      <div
        class="col-xl-3 col-sm-6"
        v-for="(item, index) in tableData"
        :key="index"
      >
        <div class="card text-center">
          <div class="card-body">
            <div class="clearfix"></div>
            <div class="mb-4" style="text-align: left">
              <h5 class="font-size-14 mb-1">
                Mesa {{ item.commentary }} - {{ item.code }}
              
              </h5>
              <h5 class="font-size-14 mb-1">
                {{ FormatDate(item.createdDate) }}
              </h5>
            </div>
            <h5 class="font-size-16 mx-auto mb-4" v-if="AllMenu == false">
              <div
                style="text-align: left"
                v-for="(
                  itemDetatils, indexDetatils
                ) in item.transactionsDetails"
                :key="indexDetatils"
              >
                <div v-if="itemDetatils.concept.isServicie">
                 
                  <b-list-group>
  <b-list-group-item> {{ itemDetatils.concept.description }}   </b-list-group-item>
 
  <b-list-group-item variant="danger"  v-for="(Element, indexElement) in itemDetatils.transactionsDetailsElement"
                  :key="indexElement"  >Sin {{ Element.detaills }} </b-list-group-item>
  
</b-list-group>
                 
            
                </div>
                
                
                 
              
                 
              </div>
            </h5>
            <h5 class="font-size-14 mx-auto mb-4" v-if="AllMenu">
              <div
                style="text-align: left"
                v-for="(
                  itemDetatils, indexDetatils
                ) in item.transactionsDetails"
                :key="indexDetatils"
              >
                <p>
                  {{ itemDetatils.concept.description }} ${{
                    itemDetatils.total
                  }}
                </p>
              </div>
              <div style="text-align: right">
                Total: {{ SetTotal(item.globalTotal) }}
              </div>
              <div style="text-align: left">
                Metodo de pago:
                <vueselect
                  v-if="AllMenu"
                  :options="PaymentMethod"
                  :reduce="(row) => row.id"
                  label="name"
                  v-model="PaymentMethodSelect"
                >
                </vueselect>
              </div>
            </h5>
          </div>

          <div class="btn-group" role="group">
            <button
              type="button"
              v-if="AllMenu == false"
              class="btn btn-outline-light text-truncate text-danger font-size-20"
              @click="
                myChangeStatus(item.id, '85685d53-d6a6-4381-944b-995ed2967faa')
              "
            >
              <i class="uil uil-cancel mr-1 text-primary"></i> Rechazar
            </button>
            <button
              v-if="AllMenu == false"
              type="button"
              class="btn btn-outline-light text-truncate text-success font-size-20"
              @click="
                myChangeStatus(item.id, '85685D53-D6A6-4381-944B-995ED2667FBA')
              "
            >
              <i class="uil uil-envelope-alt mr-1"></i> Completado
            </button>

            <button
              v-if="AllMenu"
              type="button"
              class="btn btn-outline-light text-truncate text-success font-size-20"
              @click="SendPayData(item)"
            >
              <i class="uil uil-envelope-alt mr-1"></i> Pagar
            </button>
          </div>
        </div>
      </div>
    </div>
    <!-- end row -->

    <div class="row mt-3">
      <div class="col-xl-12">
        <div class="text-center my-3">
          <button
            type="button"
            @click="myProvider()"
            class="btn btn-outline-light text-truncate text-success font-size-20"
          >
            Cargar
          </button>
        </div>
      </div>
    </div>
    <!-- end row -->
  </div>
</template>

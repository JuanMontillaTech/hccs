<script>
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
  data() {
    return {
      title: "Tablero Ordenes",
      tableData: [],
      items: [
        {
          text: "Tablero",
        },
        {
          text: "Ordenes",
          active: true,
        },
      ],
    };
  },
  middleware: "authentication",
  mounted() {
    this.myProvider();
  },
  methods: {
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },

    async myProvider() {
      let url = `Transaction/GetAllByTypeStatusIsService?TransactionsTypeId=${8}&TransactionStatusId=85685D53-D6A6-4381-944B-995ED1187FBA`;

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
  },
};
</script>

<template>
  <div>
    <PageHeader :title="title" :items="items" />

    <div class="row">
      <div
        class="col-xl-3 col-sm-6"
        v-for="(item, index) in tableData"
        :key="index"
      >
        <div class="card text-center">
          <div class="card-body">
      
            <div class="clearfix"></div>
            <div class="mb-4">
            
              <h5 class="font-size-16 mb-1">{{ item.code }}</h5>
              <h5 class="font-size-16 mb-1">
                {{ FormatDate(item.createdDate) }}
              </h5>
              <p>Cliente: {{ item.contact.name }}</p>
            </div>
            <h5 class="font-size-16 mx-auto mb-4">
              <div
                v-for="(
                  itemDetatils, indexDetatils
                ) in item.transactionsDetails"
                :key="indexDetatils"
              >
                <p v-if="itemDetatils.concept.isServicie">
                  {{ itemDetatils.concept.reference }}
                </p>
              </div>
            </h5>
          </div>

          <div class="btn-group" role="group">
            <button
              type="button"
              class="btn btn-outline-light text-truncate text-danger font-size-20"
              @click="
                myChangeStatus(item.id, '85685d53-d6a6-4381-944b-995ed2967faa')
              "
            >
              <i class="uil uil-cancel mr-1 text-primary"></i> Rechazar
            </button>
            <button
              type="button"
              class="btn btn-outline-light text-truncate text-success font-size-20"
              @click="
                myChangeStatus(item.id, '85685D53-D6A6-4381-944B-995ED2667FBA')
              "
            >
              <i class="uil uil-envelope-alt mr-1"></i> Completado
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

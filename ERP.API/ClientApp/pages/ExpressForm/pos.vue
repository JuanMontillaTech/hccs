<template>
  <div>
    <PageHeader :title="title" :items="items" />
    <div class="row">
      <div class="col-8">
        <table>
          <thead>
            <tr>
              <th class="" v-for="rowCata in CatalogueList" :key="rowCata.id">
                <button
                  v-if="rowCata.isActive"
                  class="btn btn-success rounded-circle mb-3 p-10"
                  @click="GetFilterItems(rowCata.id)"
                >
                  {{ rowCata.name }}
                </button>
              </th>
            </tr>
          </thead>
        </table>
        <div class="row">
          <div class="col-sm-5 m-1" v-for="item in SelectItems" :key="item.id">
            <template v-if="item.isActive">
              <!-- v-b-modal.modal-1  -->
              <b-button @click="GetItems(item)" class="bg-white">
                <FileImg
                  :SourceId="item.id"
                  Setclass="avatar-lg rounded-circle img-thumbnail"
                >
                </FileImg>
                <img
                  v-if="item.link"
                  :src="item.link"
                  alt
                  class="avatar-lg rounded-circle img-thumbnail"
                />
                <div class="mx-auto mb-4" v-if="!item.link">
                  <div class="text-primary">
                    <h5 class="font-size-16 mb-1" style="width: 150px">
                      {{ item.description }} - {{ SetTotal(item.priceSale) }}
                    </h5>
                  </div>
                </div>
              </b-button>
            </template>
          </div>
        </div>
      </div>
      <div class="col-4 bg-white">
        <table class="w-100">
          <tr>
            <td>
              <b-button size="sm" variant="danger" @click="clearForm()">
                <i class="fas fa-trash"></i>
              </b-button>
            </td>
            <td>
              <h4>
                Orden Total:
                <span class="bg-warring">${{ SetTotal(invoice_total) }} </span>
              </h4>
            </td>
          </tr>
          <tr>
            <td>Mesa</td>
            <td>
              <select v-model="Tablelistselect" class="col-form-label">
                <option
                  class="dropdown-item"
                  v-for="item in Tablelist"
                  :key="item.id"
                  :value="item"
                >
                  <h4>{{ item.name }}</h4>
                </option>
              </select>
            </td>
          </tr>
        </table>

        <table class="w-100">
          <tr v-for="(rowShoppingcart, index) in Shoppingcart" :key="index">
            <th>
              <b-button size="sm" variant="danger" @click="removeRow(index)">
                <i class="fas fa-trash"></i>
              </b-button>
              {{ rowShoppingcart.description }} ${{ rowShoppingcart.priceSale }}
            </th>
            <td class="text-center">
              <span class="fs-2 bg-greenTwo rounded-pill mt-1 ms-2 p-4">{{
                rowShoppingcart.count
              }}</span>
                         
            </td>

            <td>
              <b-button-group class="mt-4 mt-md-0">
                <b-button size="sm" variant="info" @click="addRow(index)">
                  <i class="fas fa-plus"></i>
                </b-button>
              </b-button-group>
            </td>
          </tr>
        </table>

        <table class="w-100">
          <tr>
            <td>Total:</td>
            <td>${{ SetTotal(invoice_total) }}</td>
          </tr>
          <tr>
            <td></td>
            <td>
              <b-button
                size="lg"
                variant="success"
                class="text-white mb-2 p-2"
                @click="SendData()"
              >
                Enviar
                <i class="fas fa-plane"></i>
              </b-button>
            </td>
          </tr>
        </table>
      </div>
    </div>
    <link rel="stylesheet" href="styles.css" />
  </div>
</template>
<script>
var numbro = require("numbro");
var moment = require("moment");
export default {
  layout: "PosLayoust",
  data() {
    return {
      FormId: "53b42d27-1a6e-4368-8da5-099ffb9e2588",
      Name: "",
      invoice_total: 0,
      TransactionType: 8,
      Shoppingcart: [],
      Tablelist: [],
      Tablelistselect: "",
      CatalogueList: [],
      Items: [],
      SelectItems: [],
      SendRow: [],
      title: "Manejo de ordenes",
      items: [
        {
          text: "Manejo de ordenes",
        },
        {
          text: "Ordenes",
          active: true,
        },
      ],
    };
  },
  created() {
    this.GetProduct();
    this.GetCatalogue();
    this.GetTablelist();
  },
  middleware: "authentication",
  methods: {
    clearForm() {
      this.Shoppingcart = [];
      this.Name = "";
      this.invoice_total = 0;
    },
    SendData() {
      if (this.Tablelistselect != "") {
        if (this.invoice_total != 0) {
          let data = {
            TransactionLocationId: this.Tablelistselect.id,
            TransactionsItems: this.Shoppingcart,
            TransactionType: this.TransactionType,
            Name: this.Name + this.Tablelistselect.name,
            Total: this.invoice_total,
            FormId: this.FormId,
          };

          let url = `Transaction/CreatePost`;
          let result = null;
          this.$axios
            .post(url, data)
            .then((response) => {
              result = response;
              this.clearForm();
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
          this.$toast.error(
            `Ingrese un producto`,
            "Informacion",
            this.izitoastConfig
          );
        }
      } else {
        this.$toast.error(
          ` Seleccione una mesa`,
          "Informacion",
          this.izitoastConfig
        );
      }
    },
    GetCatalogue() {
      this.$axios
        .get(`Catalogue/GetAll`)
        .then((response) => {
          this.CatalogueList = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetTablelist() {
      this.$axios
        .get(`TransactionLocation/GetAll`)
        .then((response) => {
          this.Tablelist = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    removeRow(index) {
      var total =
        parseFloat(this.invoice_total) -
        parseFloat(this.Shoppingcart[index].priceSale);
      this.invoice_total = total;
      let itemFound = this.Shoppingcart.find(
        (items) => items.id == this.Shoppingcart[index].id
      );
      if (itemFound.count == 1) {
        this.Shoppingcart.splice(index, 1);
      } else {
        itemFound.count = itemFound.count - 1;
      }
    },
    addRow(index) {
      let Item = this.Shoppingcart[index];
      var total = parseFloat(this.invoice_total) + parseFloat(Item.priceSale);

      this.invoice_total = total;
      let itemFound = this.Shoppingcart.find((items) => items.id == Item.id);
      itemFound.count = itemFound.count + 1;
    },
    GetProduct() {
      this.$axios
        .get(`Concept/GetAll`)
        .then((response) => {
          this.Items = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    onInput(key) {
      this.number = (this.number + key).slice(0, this.maxLength);
    },
    onDelete() {
      this.number = this.number.slice(0, this.number.length - 1);
    },
    onReset() {
      this.number = "";
    },
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    GetItems(Item) {
      let itemFound = this.Shoppingcart.find((items) => items.id == Item.id);

      if (itemFound == undefined) {
        this.Shoppingcart.push({
          id: Item.id,
          count: 1,
          description: Item.description,
          priceSale: Item.priceSale,
        });
      } else {
        itemFound.count = itemFound.count + 1;
      }
      var total = parseFloat(Item.priceSale) + parseFloat(this.invoice_total);
      this.invoice_total = total;
    },

    GetFilterItems(SelectionCatalogueId) {
      this.SelectItems = [];
      this.SelectItems = this.Items.filter(
        (element) => element.catalogueId == SelectionCatalogueId
      );
    },
  },
};
</script>

<template>
  <div>
    <!-- Modal for create a contact -->

    <b-modal size="lg" title="Formulario de Banco" header-bg-variant="#000" v-model="ShowModalCreate" hide-footer>
      <div class="container">
        <div class="row">
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Nombre del Banco">
              <b-form-input v-model="bank.Name" size="sm" trim></b-form-input>
              <p class="text-danger text-size-required m-0" v-if="$v.bank.Name.$error">
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Número de Cuenta">
              <b-form-input v-model="bank.AccountNumber" size="sm" trim></b-form-input>
              <p class="text-danger text-size-required m-0" v-if="$v.bank.AccountNumber.$error">
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-sm-12">
            <div class="form-group">
              <b-form-group label="Cuenta">
                <vueselect :options="LedgerAccounts" v-model="bank.LedgerAccountId" :reduce="(row) => row.id"
                  label="name"></vueselect>
                <p class="text-danger text-size-required m-0" v-if="$v.bank.LedgerAccountId.$error">
                  Campo requerido.
                </p>
              </b-form-group>
            </div>
          </div>
          <div class="col-lg-6 col-sm-12">
            <div class="form-group">
              <b-form-group label="Moneda">




                <vueselect :options="CurrencyList" v-model="bank.CurrencyId" :reduce="(row) => row.id" label="name">
                  <template #option="{ name, code, country }">
                    <h5 style="margin: 0">{{ name }} ${{ code }}</h5>
                    <em>{{ country }} </em>
                  </template>

                </vueselect>
                <p class="text-danger text-size-required m-0" v-if="$v.bank.CurrencyId.$error">
                  Campo requerido.
                </p>
              </b-form-group>
            </div>
          </div>
          <div class="modal-footer">
            <div>
              <b-button-group class="mt-4 mt-md-0">
                <b-button variant="danger" class="btn" @click="ShowModalCreate = !ShowModalCreate">
                  <i class="bx bx-x"></i> Cerrar
                </b-button>
                <b-button variant="success" class="btn" @click="saveBank()">
                  <i class="bx bx-save"></i> Guardar
                </b-button>
              </b-button-group>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <!-- Modal for show contact details -->
    <b-modal size="lg" title="Formulario de Banco" v-model="ShowModalDetails" hide-footer>
      <div class="container">
        <div class="row">
          <div class="row justify-content-end w-100 gx-2">
            <div class="col-2 p-2">
              <b-button variant="danger" class="btn" @click="ShowModalDetails = !ShowModalDetails">
                <i class="bx bx-x"></i> Cerrar
              </b-button>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <!-- Modal for update contact -->
    <b-modal size="lg" title="Formulario de Banco" v-model="ShowModalEdit" hide-footer>
      <div class="container">
        <div class="row">
          <div class="row justify-content-end w-100">
            <div class="d-flex justify-content-end">
              <b-button-group class="mt-4 mt-md-0">
                <b-button variant="danger" class="btn" @click="ShowModalEdit = !ShowModalEdit">
                  <i class="bx bx-x"></i> Cerrar
                </b-button>
                <b-button variant="success" class="btn" @click="editContact()">
                  <i class="bx bx-save"></i> Guardar
                </b-button>
              </b-button-group>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <h4>Listado de Bancos</h4>
        </div>
        <div class="btn-group" role="group" aria-label="Basic example">
          <a title="Nuevo Registro" @click="showModal()" class="btn btn-primary btn-sm text-white">
            <i class="fas fa-file"></i>
            Nuevo</a>

          <a id="_btnRefresh" @click="GetAllRows()" class="btn btn-light border btn-sm text-black-50 btnRefresh"
            name="_btnRefresh"><i class="fas fa-sync-alt"></i> Actualizar Datos</a>
        </div>
      </div>
    </nav>

    <vue-good-table :columns="columns" :rows="rows" :search-options="{
      enabled: true,
    }" :pagination-options="{
  enabled: true,
  mode: 'records',
}">
      <template slot="table-row" slot-scope="props">
         

        <span v-if="props.column.field == 'action'">
          <b-button variant="light" size="sm" @click="showContact(props.row)">
            <i class="fas fa-eye"></i>
          </b-button>
          <b-button variant="danger" size="sm" @click="removeBank(props.row)">
            <i class="fas fa-trash"></i>
          </b-button>
          <b-button variant="info" size="sm" @click="editBankModal(props.row)">
            <i class="fas fa-edit"></i>
          </b-button>
        </span>
          <span v-if="props.column.field == 'accountNumber'">
          {{props.row.accountNumber}} ${{props.row.currencys.code}} <em>( {{props.row.currencys.name}} {{props.row.currencys.country}}   )</em>
        </span>
        <span v-else>
          {{ props.formattedRow[props.column.field] }}
        </span>
      </template>
    </vue-good-table>
  </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
export default {
  data() {
    return {
      ShowModalCreate: false,
      ShowModalEdit: false,
      ShowModalDelete: false,
      ShowModalDetails: false,
      DeleteStatus: false,
      bank: {
        CurrencyId: null,
        Name: null,
        AccountNumber: null,
        LedgerAccountId: null,
        Currencys: null,
        LedgerAccount: null,
      },
      LedgerAccounts: [],
      CurrencyList: [],
      izitoastConfig: {
        position: "topRight",
      },
      ControllerURL: "LedgerAccount/GetAll",
      columns: [
        {
          label: "Nombre",
          field: "name",
        },
        {
          label: "Número de cuenta",
          field: "accountNumber",
        },

        {
          label: "Acciones",
          field: "action",
        },
      ],
      rows: [],
    };
  },
  validations: {
    bank: {
      Name: {
        required,
      },
      CurrencyId: {
        required,
      },
      AccountNumber: {
        required,
      },
      LedgerAccountId: {
        required,
      },
    },
  },
  created() {
    this.GetAllRows();
    this.getLeaderAccount();
    this.getCurrencyList();
  },
  methods: {
    async getLeaderAccount() {

      this.$axios
        .get(this.ControllerURL)
        .then((response) => {
          this.LedgerAccounts = response.data.data;

        })
        .catch((response) => {
          alert(response.data);
        });
    },
    async getCurrencyList() {

      this.$axios
        .get(this.ControllerURL)
        .then((response) => {
          this.CurrencyList = response.data.data;

        })
        .catch((response) => {
          alert(response.data);
        });
    },
    GetAllRows() {
      this.$axios
        .get("Bank/GetAll")
        .then((response) => {
          this.rows = response.data.data;

        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    showModal() {
      this.ShowModalCreate = true;
    },
    saveBank() {

      this.$v.$touch();
      if (this.$v.$invalid) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        this.ShowModalCreate = false;
        this.post();
      }
    },
    showContact(bank) {
      this.bank = bank;
      this.ShowModalDetails = true;
    },
    removeBank(bank) {
      this.delete(bank.id);
      this.GetAllRows();
    },
    editBankModal(bank) {
      this.ShowModalEdit = true;
      this.bank = bank;
    },
    editContact() {
      this.put(this.bank);
    },
    post() {
      console.log(this.bank);
      let Data2 = {
        accountNumber: "wert",
        currencyId: "18aa6416-3dc0-427b-a09b-0789d1c0a38f",
        currencys: null,
        ledgerAccount: null,
        ledgerAccountId: "cb58d719-818a-4327-86c6-02ea7e4538e1",
        name: "ewrt",
      }


      this.$axios
        .post(this.ControllerURL, this.bank)
        .then((response) => {
          resolve(response);
          this.$toast.success(
            "El Banco ha sido creado correctamente.",
            "EXITO",
            this.izitoastConfig
          );
          this.GetAllRows();
          this.clearForm();
        })
        .catch((error) => {

          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });

    },
    async put(data) {


      this.$axios
        .put(this.ControllerURL, data)
        .then((response) => {
          resolve(response);
          this.$toast.success(
            "El Banco ha sido actualizado correctamente.",
            "EXITO",
            this.izitoastConfig
          );
          this.GetAllRows();
          this.ShowModalEdit = false;
        })
        .catch((error) => {

          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });

    },
    async delete(id) {
      let result = false;
      this.$toast.question(
        "Esta seguro que quiere eliminar este banco?",
        "PREGUNTA",
        {
          timeout: 20000,
          close: false,
          overlay: true,
          toastOnce: true,
          id: "question",
          zindex: 999,
          position: "center",
          buttons: [
            [
              "<button><b>YES</b></button>",
              function (instance, toast) {
                instance.hide({ transitionOut: "fadeOut" }, toast, "button");

                axios
                  .delete(`Bank/Delete?id=${id}`, {
                    headers: {
                      Authorization: `${localStorage.getItem("authUser")}`,
                    },
                  })
                  .then((response) => {
                    alert(
                      "EXITO: El Registro ha sido eliminado correctamente."
                    );
                    location.reload();
                  })
                  .catch((error) => alert(error));
              },
              true,
            ],
            [
              "<button>NO</button>",
              function (instance, toast) {
                instance.hide({ transitionOut: "fadeOut" }, toast, "button");
              },
            ],
          ],
        }
      );
      this.GetAllRows();
    },
    clearForm() {
      (this.bank = {
        CurrencyId: null,
        Name: null,
        AccountNumber: null,
        LedgerAccountId: null,
      }),
        (this.ShowModalCreate = false);
    },
  },
};
</script>

<style>
.text-size-required {
  font-size: 12px;
}
</style>

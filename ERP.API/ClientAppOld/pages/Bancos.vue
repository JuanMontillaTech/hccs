<template>
  <div class="container">
    <!-- Modal for create bank -->
    <b-modal
      size="lg"
      title="Formulario de Bancos"
      header-bg-variant="#000"
      v-model="ShowModalCreate"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Tipo de cuenta">
              <b-form-select
                v-model="bank.AccountType"
                :options="AccountTypeOptions"
                :state="$v.bank.AccountType.$error ? false : null"
                size="sm"
              ></b-form-select>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.AccountType.$error"
              >
                Tipo de cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Nombre de la cuenta">
              <b-form-input
                v-model="bank.AccountName"
                size="sm"
                :state="$v.bank.AccountName.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.AccountName.$error"
              >
                Nombre de la cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Numero de la cuenta">
              <b-form-input
                v-model="bank.AccountNumber"
                size="sm"
                :state="$v.bank.AccountNumber.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.AccountNumber.$error"
              >
                Numero de la cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Saldo inicial">
              <b-form-input
                v-model="bank.InitialBalance"
                size="sm"
                :state="$v.bank.InitialBalance.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.InitialBalance.$error"
              >
                Saldo inicial requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Fecha">
              <b-form-datepicker
                placeholder="Fecha"
                v-model="bank.Date"
                class="mb-2"
              ></b-form-datepicker>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.Date.$error"
              >
                Fecha requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Descripcion">
              <b-form-textarea v-model="bank.Description"></b-form-textarea>
            </b-form-group>
          </div>
          <div class="row justify-content-end w-100 gx-2">
            <div class="col-2 p-2">
              <b-button
                variant="danger"
                class="w-100"
                @click="ShowModalCreate = !ShowModalCreate"
                >Cancelar</b-button
              >
            </div>
            <div class="col-3 p-2">
              <b-button
                class="w-100"
                style="background-color: #457b9d"
                @click="saveBank()"
              >
                <span>Guardar</span>
              </b-button>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <!-- Modal for bank details -->
    <b-modal
      size="lg"
      title="Formulario de Bancos"
      v-model="ShowModalDetails"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Tipo de cuenta">
              <b-form-select
                v-model="bank.AccountType"
                :options="AccountTypeOptions"
                :state="$v.bank.AccountType.$error ? false : null"
                size="sm"
                disabled
              ></b-form-select>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.AccountType.$error"
              >
                Tipo de cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Nombre de la cuenta">
              <b-form-input
                v-model="bank.AccountName"
                size="sm"
                :state="$v.bank.AccountName.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.AccountName.$error"
              >
                Nombre de la cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Numero de la cuenta">
              <b-form-input
                v-model="bank.AccountNumber"
                size="sm"
                :state="$v.bank.AccountNumber.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.AccountNumber.$error"
              >
                Numero de la cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Saldo inicial">
              <b-form-input
                v-model="bank.InitialBalance"
                size="sm"
                :state="$v.bank.InitialBalance.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.InitialBalance.$error"
              >
                Saldo inicial requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Fecha">
              <b-form-datepicker
                placeholder="Fecha"
                v-model="bank.Date"
                class="mb-2"
                disabled
              ></b-form-datepicker>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.Date.$error"
              >
                Fecha requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Descripcion">
              <b-form-textarea v-model="bank.Description"></b-form-textarea>
            </b-form-group>
          </div>
          <div class="row justify-content-end w-100 gx-2">
            <div class="col-2 p-2">
              <b-button
                variant="danger"
                class="w-100"
                @click="ShowModalDetails = !ShowModalDetails"
                >Cancelar</b-button
              >
            </div>
            <div class="col-3 p-2">
              <b-button
                class="w-100"
                style="background-color: #457b9d"
                disabled
                @click="saveBank()"
              >
                <span>Guardar</span>
              </b-button>
            </div>
          </div>
        </div>
      </div>
    </b-modal>
    <!-- Modal for edit bank -->
    <b-modal
      size="lg"
      title="Formulario de Bancos"
      v-model="ShowModalEdit"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Tipo de cuenta">
              <b-form-select
                v-model="bank.AccountType"
                :options="AccountTypeOptions"
                :state="$v.bank.AccountType.$error ? false : null"
                size="sm"
              ></b-form-select>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.AccountType.$error"
              >
                Tipo de cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Nombre de la cuenta">
              <b-form-input
                v-model="bank.AccountName"
                size="sm"
                :state="$v.bank.AccountName.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.AccountName.$error"
              >
                Nombre de la cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Numero de la cuenta">
              <b-form-input
                v-model="bank.AccountNumber"
                size="sm"
                :state="$v.bank.AccountNumber.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.AccountNumber.$error"
              >
                Numero de la cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Saldo inicial">
              <b-form-input
                v-model="bank.InitialBalance"
                size="sm"
                :state="$v.bank.InitialBalance.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.InitialBalance.$error"
              >
                Saldo inicial requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Fecha">
              <b-form-datepicker
                placeholder="Fecha"
                v-model="bank.Date"
                class="mb-2"
              ></b-form-datepicker>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.Date.$error"
              >
                Fecha requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Descripcion">
              <b-form-textarea v-model="bank.Description"></b-form-textarea>
            </b-form-group>
          </div>
          <div class="row justify-content-end w-100 gx-2">
            <div class="col-2 p-2">
              <b-button
                variant="danger"
                class="w-100"
                @click="ShowModalEdit = !ShowModalEdit"
                >Cancelar</b-button
              >
            </div>
            <div class="col-3 p-2">
              <b-button
                class="w-100"
                style="background-color: #457b9d"
                @click="editBank()"
              >
                <span>Guardar</span>
              </b-button>
            </div>
          </div>
        </div>
      </div>
    </b-modal>
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <div>Listado de {{ $options.name }}</div>
        </div>
        <div class="btn-group" role="group" aria-label="Basic example">
          <a
            title="Nuevo Registro"
            @click="showModal()"
            class="btn btn-primary btn-sm text-white"
          >
            <fa icon="file" class="ml-1"></fa>
            Nuevo</a
          >

          <a
            id="_btnRefresh"
            @click="GetAllRows()"
            class="btn btn-light border btn-sm text-black-50 btnRefresh"
            name="_btnRefresh"
            ><i class="fas fa-sync-alt"></i> Actualizar Datos</a
          >
        </div>
      </div>
    </nav>
    <vue-good-table
      :columns="columns"
      :rows="rows"
      :search-options="{
        enabled: true,
      }"
      :pagination-options="{
        enabled: true,
        mode: 'records',
      }"
    >
      <template slot="table-row" slot-scope="props">
        <span v-if="props.column.field == 'action'">
          <b-button class="btn btn-light btn-sm" @click="showBank(props.row)">
            <fa icon="eye"></fa>
          </b-button>
          <b-button class="btn btn-light btn-sm" @click="removeBank(props.row)">
            <fa icon="trash"></fa>
          </b-button>
          <b-button
            class="btn btn-light btn-sm"
            @click="editBankModal(props.row)"
          >
            <fa icon="edit"></fa>
            ></b-button
          >
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
  layout: "TheSlidebar",
  name: "Bancos",
  data() {
    return {
      ShowModalCreate: false,
      ShowModalEdit: false,
      ShowModalDelete: false,
      ShowModalDetails: false,
      bank: {
        AccountType: "",
        AccountName: "",
        AccountNumber: "",
        InitialBalance: "",
        Date: "",
        Description: "",
      },
      AccountTypeOptions: [
        { value: "Banco", text: "Banco" },
        { value: "Tarjeta de credito", text: "Tarjeta de credito" },
        { value: "Efectivo", text: "Efectivo" },
      ],
      izitoastConfig: {
        position: "topRight",
      },

      columns: [
        {
          label: "Nombre",
          field: "AccountName",
        },
        {
          label: "Número de la cuenta",
          field: "AccountNumber",
          type: "number",
        },
        {
          label: "Descripción",
          field: "Description",
        },
        {
          label: "Balance",
          field: "InitialBalance",
        },
        {
          label: "Acciones",
          field: "action",
        },
      ],
      rows: [
        {
          id: 1,
          AccountName: "Banco 1",
          AccountNumber: "403000412",
          Description: "Lorem Ipsum",
          InitialBalance: "0.00",
        },
        {
          id: 2,
          AccountName: "Banco 2",
          AccountNumber: "403000412",
          Description: "Lorem Ipsum",
          InitialBalance: "0.00",
        },
        {
          id: 3,
          AccountName: "Banco 3",
          AccountNumber: "403000412",
          Description: "Lorem Ipsum",
          InitialBalance: "0.00",
        },
        {
          id: 4,
          AccountName: "Banco 4",
          AccountNumber: "403000412",
          Description: "Lorem Ipsum",
          InitialBalance: "0.00",
        },
        {
          id: 5,
          AccountName: "Banco 5",
          AccountNumber: "403000412",
          Description: "Lorem Ipsum",
          InitialBalance: "0.00",
        },
        {
          id: 6,
          AccountName: "Banco 6",
          AccountNumber: "403000412",
          Description: "Lorem Ipsum",
          InitialBalance: "0.00",
        },
        {
          id: 7,
          AccountName: "Banco 7",
          AccountNumber: "403000412",
          Description: "Lorem Ipsum",
          InitialBalance: "0.00",
        },
        {
          id: 8,
          AccountName: "Banco 8",
          AccountNumber: "403000412",
          Description: "Lorem Ipsum",
          InitialBalance: "0.00",
        },
        {
          id: 9,
          AccountName: "Banco 9",
          AccountNumber: "403000412",
          Description: "Lorem Ipsum",
          InitialBalance: "0.00",
        },
        {
          id: 10,
          AccountName: "Banco 10",
          AccountNumber: "403000412",
          Description: "Lorem Ipsum",
          InitialBalance: "0.00",
        },
      ],
    };
  },
  validations: {
    bank: {
      AccountNumber: {
        required,
      },
      AccountType: {
        required,
      },
      AccountName: {
        required,
      },
      AccountNumber: {
        required,
      },
      InitialBalance: {
        required,
      },
      Date: {
        required,
      },
    },
  },
  methods: {
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
        this.post(this.bank);
      }
    },
    showBank(bank) {
      this.bank = bank;
      this.ShowModalDetails = true;
    },
    editBankModal(bank) {
      this.ShowModalEdit = true;
      this.bank = bank;
    },
    removeBank(bank) {
      this.delete(bank.id);
    },
    editBank() {
      this.put(this.bank);
    },
    async post(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .post(process.env.devUrl + "Contact/Create", data, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El Banco ha sido creado correctamente.",
              "EXITO",
              this.izitoastConfig
            );
          })
          .catch((error) => {
            reject(error);
            this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          });
      });
    },
    async put(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .put(process.env.devUrl + "Contact/Update", data, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El Banco ha sido actualizado correctamente.",
              "EXITO",
              this.izitoastConfig
            );
          })
          .catch((error) => {
            reject(error);
            this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          });
      });
    },
    async delete(id) {
      return new Promise((resolve, reject) => {
        this.$axios
          .delete(process.env.devUrl + `Contact/Delete/${id}`, {
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El Banco ha sido eliminado correctamente.",
              "EXITO",
              this.izitoastConfig
            );
          })
          .catch((error) => {
            reject(error);
            this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          });
      });
    },
    clearForm() {
      for (const key in this.contact) {
        this.contact[key] = "";
      }
      this.ShowModalCreate = false;
    },
  },
};
</script>

<style>
.modal-header {
  background-color: #457b9d !important;
  color: #fff;
}
.text-size-required {
  font-size: 12px;
}
</style>

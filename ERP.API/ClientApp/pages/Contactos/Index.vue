<template>
  <div>
    <div class="card">
      <div class="card-header bg-Cprimary">
        Listado de {{ this.$options.name }}
      </div>
      <div class="card-body">
        <div class="btn-group" role="group" aria-label="Basic example">
          <a
            title="Nuevo Registro"
            v-on:click="showModal"
            class="btn btn-primary btn-sm text-white"
            >
            <fa icon="file" class="ml-1"></fa>
            Nuevo</a
          >

          <a
            id="_btnRefresh"
            v-on:click="GetAllRows()"
            class="btn btn-light btn-sm text-black-50 btnRefresh"
            name="_btnRefresh"
            ><i class="fas fa-sync-alt"></i> Actualizar Datos</a
          >
        </div>
        <vue-good-table
          :columns="columns"
          :rows="Records"
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
              <b-button
                class="btn btn-light btn-sm"
                @click="RemoveRecord(props.row)"
              >
                <i class="fa fa-trash"></i>
              </b-button>
              <b-button
                class="btn btn-light btn-sm"
                @click="EditShow(props.row)"
              >
                <i class="fa fa-edit"></i
              ></b-button>
            </span>
            <span v-else>
              {{ props.formattedRow[props.column.field] }}
            </span>
          </template>
        </vue-good-table>
      </div>
    </div>
    <b-modal
      id="newModal"
      v-model="ShowModelCreate"
      :title="fromTitle"
      hide-footer
    >
      <b-container fluid>
        <b-row class="my-1" v-for="type in columns" :key="type">
          <template v-if="type.field != 'action'">
          <b-col sm="3">
            <label :for="`type-${type.field}`"> {{ type.label }} :</label>
          </b-col>
          <b-col sm="9">
            <b-form-input
              :id="`type-${type.field}`"
              v-model="Model[type.field]"
              :type="`${type.type}`"
            ></b-form-input>
          </b-col>
          </template>
        </b-row>
      </b-container>

      <div class="modal-footer">
        <button
          type="button"
          class="btn btn-light btn-sm text-black-50"
          v-on:click="HideModal()"
        >
          <i class="fas fa-list"></i> Cerrar
        </button>
        <button
          type="button"
          class="btn btn-success btn-sm text-white btnSave"
          v-on:click="Save"
        >
          <fa icon="save" class="ml-1"></fa> Guardar
        </button>
      </div>
    </b-modal>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "Contactos",
  layout: 'TheSlidebar',
  data() {
    return {
      controller:"Contact",
      columns: [
        {
          label: "",
          field: "action",
        },
        {
          label: "Nombre",
          field: "name",
           type: 'text',
        },
        {
          label: "Identificación",
          field: "identity",
           type: 'text',
        },
        {
          label: "Celular",
          field: "cellPhone",
           type: 'tel',
        },
        {
          label: "Tel.1",
          field: "phone1",
           type: 'tel',
        },
        {
          label: "Tel.2",
          field: "phone2",
          type: 'tel',
        },
        {
          label: "Dirección",
          field: "address",
        },
      ],
      ShowModelCreate: false,
      ShowModelEdit: false,
      ShowModelDelete: false,
      Records: [],
      Model: {
        id: null,
        name: "",
        identity: "",
        cellPhone: "",
        phone1: "",
        phone2: "",
        address: "",
      },
      fromTitle: "Crear",
    };
  },
  created: function () {
    this.GetAllRows();
  },
  methods: {
    async clearData() {
      this.fromTitle = "Editar Regisro";
      this.Model.name = "";
      this.Model.id = null;
    },
    async EditShow(item) {
      let EditModel = item;
      this.Model = EditModel;
      this.fromTitle = "Editar Regisro";
      this.ShowModelCreate = true;
    },
    async Save() {
      if (this.Model.id == null) {
        this.AddRecord();
      } else {
        this.EditRecord();
      }
    },
    async EditRecord() {
      let url = `https://localhost:5001/api/${this.controller}/Update`;
      let result = null;
      axios
        .put(url, this.Model, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          if (response.data.succeeded) {
            this.HideModal();
          }
        })
        .catch((error) => {
          result = error;
        });
      this.ShowModelEdit = false;
    },
    async RemoveRecord(item) {
      let url = `https://localhost:5001/api/${this.controller}/Delete?id=${item.id}`;
      let result = null;
      axios
        .delete(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          if (response.data.succeeded) {
            this.HideModal();
          }
        })
        .catch((error) => {
          result = error;
        });
    },
    async AddRecord() {
      let url = `https://localhost:5001/api/${this.controller}/Create`;
      let result = null;
      axios
        .post(url, this.Model, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          if (response.data.succeeded) {
            this.HideModal();
          }
        })
        .catch((error) => {
          result = error;
        });
    },
    async GetAllRows() {
      let url = `https://localhost:5001/api/${this.controller}/GetAll`;
      let result = null;
      axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;

          this.Records = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async showModal() {
      this.clearData();
      this.fromTitle = "Crear Regisro";
      this.ShowModelCreate = true;
    },
    async HideModal() {
      this.GetAllRows();
      this.ShowModelCreate = false;
    },
  },
};
</script>

<style></style>

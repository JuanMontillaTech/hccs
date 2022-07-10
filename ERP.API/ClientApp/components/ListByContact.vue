<template>
  <div>
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <h4>Listado de {{ Title }}</h4>
        </div>
        <div class="btn-group" role="group" aria-label="Basic example">
          <a
            id="_btnRefresh"
            @click="GetAllSchemaRows()"
            class="btn btn-light border btn-sm text-black-50 btnRefresh"
            name="_btnRefresh"
            ><i class="fas fa-sync-alt"></i> Actualizar Datos</a
          >
        </div>
      </div>
    </nav>
    {{ rows }}
    <table class="table">
      <thead>
        <tr>
            <th>No.</th>
          <th>Code</th>
          <th>Fecha</th>

          <th>Referencia</th>
          <th>Total</th>
          <th class="text-right" style="width: 120px">Balance</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, idx) in rows" :key="item.id">
        <td> .{{ idx.code }} .</td>
          <td>
           {{ item.code }}
           
          </td>
          <td  >{{ item.date }}</td>
          <td >{{ item.reference }}</td>
          <td >{{ item.globalTotal }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
var numbro = require("numbro");
var moment = require("moment");
export default {
  head() {
    return {
      title: `${this.Title} | ERP Cardenal Sancha`,
    };
  },

  data() {
    return {
      izitoastConfig: {
        position: "topRight",
      },
      rows: [],
      columns: [
        {
          field: "code",
          label: "Codigo",
          sortable: true,
        },
        {
          field: "reference",

          label: "Referencia",
        },
        {
          field: "date",
          label: "Fecha",
        },
        {
          field: "globalTotal",
          label: "Total",
          sortable: true,
        },
        {
          label: "Acciones",
          field: "action",
        },
      ],
    };
  },
  props: ["Title", "Id"],
  created() {
    this.GetAllSchemaRows();
  },
  methods: {
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    GetDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },

    showSchema(id) {
      this.$router.push({
        path: "/formulario/detail",
        query: { id: id, type: this.Form },
      });
    },
    GetAllSchemaRows() {
      this.rows = [];
      this.$axios
        .get(
          
            "Transaction/GetAllByContact?ContactId=" +
            this.Id,
          {
            headers: {
              "Content-Type": "application/json",
             Authorization: `${localStorage.getItem("authUser")}`,
            },
          }
        )
        .then((response) => {
          console.log(response.data);
          this.rows = response.data.data;
          //   response.data.data.map((schema) => {
          //     let objSchema = schema;
          //     objSchema.date = this.GetDate(schema.date);
          //     objSchema.globalTotal = this.SetTotal(schema.globalTotal);
          //     if (objSchema.isActive) this.rows.push(objSchema);
          //   });
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
  },
};
</script>

<style>
.text-size-required {
  font-size: 12px;
}
</style>

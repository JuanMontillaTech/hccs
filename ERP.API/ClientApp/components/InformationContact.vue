<template>
  <div>
    <div v-if="ContactData.id != null">
      <div class="row ml-0 mb-3">
        <div class="col-lg-12 col-md-12 col-sm-12">
          <hr class="new1" />
          <b-form-group id="input-group-2" label-for="input-2">
            <h4 class="card-title">Impuestos del Contacto</h4>
            <b-alert show variant="warning" v-if="ContactData.documentNumber === null">  No es posible generar facturas con impuestos. Por favor, asigne un número fiscal válido para
                    continuar.</b-alert>
            <table class="table table-striped">
              <thead>
                <tr>
                  <th>Nombre</th>
                  <th>Número Identificación</th>
                  <th>Tipo Comprobante</th>
                  <th class="text-right">Grupo Impuestos</th>
                  <th>Teléfono</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>{{ ContactData.name }}</td>
                  <td v-if="ContactData.documentNumber != null">
                    {{ ContactData.documentNumber }}
                  </td>
                  <td v-else>
                  No existe un número de identificación
                  </td>
                  <td>{{ ContactData.numeration.name }}</td>
                  <td>{{ GroupTaxes.description }}</td>
                  <td>{{ ContactData.phone1 }} {{ ContactData.phone2 }}</td>
                </tr>
              </tbody>
            </table>
          </b-form-group>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "InfiniteScroll",

  data: () => ({
    ContactData: { id: null },
    GroupTaxes: { id: null },
  }),
  props: ["ConctactId"],

  computed: {},
  watch: {
    ConctactId: function (id) {
      this.onSearch(this.ConctactId);
    },
  },
  mounted() {
    if (this.IdConctact != null) {
      this.ContactData.id = this.IdConctact;
      this.onSearch(this.ConctactId);
    }
  },
  methods: {
    onSearch(id) {
      var url = `Contact/GetById?Id=${id}`;
      this.$axios
        .get(url)
        .then((response) => {
          this.ContactData = response.data.data;
          this.onSearchTaxt(this.ContactData.taxesId);
        })
        .catch((error) => {
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    onSearchTaxt(id) {
      if (id == null) return;
      var url = `GroupTaxes/GetById?Id=${id}`;
      this.$axios
        .get(url)
        .then((response) => {
          this.GroupTaxes = response.data.data;
        })
        .catch((error) => {
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
  },
};
</script>

<style scoped>
.loader {
  text-align: center;
  color: #bbbbbb;
}
</style>

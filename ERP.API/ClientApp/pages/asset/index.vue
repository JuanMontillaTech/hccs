<template>
  <div>
    <div class="card">
      <div class="card-body">
        <h4>Listado de Activos</h4>
        <div class="row">
          <div class="col-md-4 mb-2">
            <input type="text" class="form-control" placeholder="Buscar..." v-model="searchQuery" @input="searchAssets">
          </div>
          <div class="col-md-8 mb-2 text-right">
            <button class="btn btn-primary" @click="createNewAsset">
              <i class="bx bx-plus"></i> Nuevo Activo
            </button>
          </div>
        </div>
        <b-table striped hover :items="assets" :fields="fields" :per-page="perPage" :current-page="currentPage"
          :filter="searchQuery">
          <template #cell(actions)="row">
            <b-button size="sm" @click="editAsset(row.item.id)">
              <i class="bx bx-edit"></i> Editar
            </b-button>
            <b-button size="sm" variant="danger" @click="deleteAsset(row.item.id)">
              <i class="bx bx-trash"></i> Eliminar
            </b-button>
          </template>
        </b-table>
        <b-pagination v-model="currentPage" :total-rows="totalRows" :per-page="perPage" align="center"></b-pagination>
      </div>
    </div>
  </div>
</template>

<script>
import Swal from "sweetalert2";
import moment from "moment/moment";
import numbro from "numbro";

export default {
  data() {
    return {
      assets: [],
      fields: [
        { key: 'name', label: 'Nombre' },
        { key: 'assetClassName', label: 'Clase de Activo' },
        { key: 'locationName', label: 'Ubicación' },
        { key: 'acquisitionDate', label: 'Fecha de Adquisición' },
        { key: 'acquisitionCost', label: 'Costo' },
        { key: 'status', label: 'Estado' },
        { key: 'actions', label: 'Acciones' }
      ],
      searchQuery: "",
      currentPage: 1,
      perPage: 10,
      totalRows: 0
    };
  },
  async mounted() {
    await this.fetchAssets();
  },
  methods: {
    FormatMoney(globalTotal) {
      return numbro(globalTotal).format("$0,0.00");
    },
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },

    async fetchAssets() {
      try {
        const response = await this.$axios.get('Asset/GetAll');
        this.assets = response.data.data.map(asset => ({
          ...asset,
          assetClassName: asset.assetClass.name,
          locationName: asset.location.name,
          acquisitionDate : this.FormatDate(asset.acquisitionDate),
          acquisitionCost : this.FormatMoney(asset.acquisitionCost)
        }));
        this.totalRows = this.assets.length;
      } catch (error) {
        this.$toast.error(error.message, "Error al obtener activos", this.izitoastConfig);
      }
    },
    createNewAsset() {
      this.$router.push("/asset/create");
    },
    editAsset(id) {
      this.$router.push(`/asset/Edit?id=${id}`);
    },
    async deleteAsset(id) {
      try {
        const result = await Swal.fire({
          title: '¿Estás seguro?',
          text: "Esta acción no se puede deshacer.",
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#3085d6',
          cancelButtonColor: '#d33',
          confirmButtonText: 'Sí, eliminar',
          cancelButtonText: 'Cancelar'
        });

        if (result.isConfirmed) {
          await this.$axios.delete(`Asset/Delete/${id}`);
          this.$toast.success("Activo eliminado correctamente.", "Éxito", this.izitoastConfig);
          await this.fetchAssets();
        }
      } catch (error) {
        this.$toast.error(error.message, "Error al eliminar el activo", this.izitoastConfig);
      }
    },
    searchAssets() {
      // Puedes implementar lógica de búsqueda adicional aquí si es necesario
      // Por ahora, la búsqueda se realiza automáticamente por la directiva `v-model` en el input
    }
  }
};
</script>

<style>
/* Estilos opcionales */
</style>
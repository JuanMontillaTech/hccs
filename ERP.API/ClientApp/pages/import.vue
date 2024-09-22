<template>
    <div class="card">
      <h4>Importar semestre</h4>
      <div class="row">
        <div class="col-2">
          <h4 class="card-title">Año:</h4>
          <input type="text" class="form-control" />
        </div>
        <div class="col-2">
          <h4 class="card-title">Total Caja año Anterior:</h4>
          <input type="text" class="form-control" />
        </div>
        <div class="col-4">
          <h4 class="card-title">Archivo:</h4>
          <input type="file" @change="cargarArchivo" class="form-control" accept=".csv" />
        </div>
        <div class="col-2">
        <b-button-group v-if="documentData.length > 0">
        <b-button variant="success" class="btn" size="sm" @click="enviarTodosLosDatos()">
          <i class="bx bx-save"></i> Enviar todo 
        </b-button>
      </b-button-group>
    </div>
      </div>
  
      <div>
        <table class="table" v-if="documentData.length > 0">
          <thead>
            <tr>
              <th v-for="header in headers" :key="header">{{ header }}</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(fila, index) in documentData" :key="index" > 
              <td v-for="header in headers" :key="header">{{ fila[header] }}</td>
            </tr>
          </tbody>
        </table>
      </div>
      <b-button-group v-if="documentData.length > 0">
        <b-button variant="success" class="btn" size="sm" @click="enviarTodosLosDatos()">
          <i class="bx bx-save"></i> Enviar todo 
        </b-button>
      </b-button-group>
    </div>
  </template>
  
  <script>
  import Papa from 'papaparse';
  import axios from 'axios';
  
  export default {
    data() {
      return {
        documentData: [],
        headers: []
      };
    },
    methods: {
      cargarArchivo(event) {
        const archivo = event.target.files[0];
        Papa.parse(archivo, {
          header: true,
          complete: (resultados) => {
            this.headers = resultados.meta.fields;
            this.documentData = resultados.data;
          }
        });
      },
      async enviarDatosAApi(fila) {
        try {
          const response = await axios.post("Import/Create", fila);
          console.log(response);
          if (response.data.succeeded) {
            this.$toast.success(
              "El Registro ha sido creado correctamente.",
              "ÉXITO"
            );
            // this.GoBack(); // Asegúrate de que GoBack() esté definido en tu componente
          } else {
            this.$toast.info(
              `${response.data.friendlyMessage}`,
              "Información",
              this.izitoastConfig // Asegúrate de que izitoastConfig esté definido
            );
          }
        } catch (error) {
          console.error('Error al enviar datos a la API:', error);
          this.$toast.error(
            "Error al enviar datos. Por favor, inténtalo de nuevo.",
            "Error",
            this.izitoastConfig
          );
        }
      },
      async enviarTodosLosDatos() {
        try {
          const response = await axios.post("Import/Create", this.documentData);
          console.log(response);
          if (response.data.succeeded) {
            this.$toast.success(
              "Todos los registros han sido creados correctamente.",
              "ÉXITO"
            );
            // this.GoBack(); // Asegúrate de que GoBack() esté definido en tu componente
          } else {
            this.$toast.info(
              `${response.data.friendlyMessage}`,
              "Información",
              this.izitoastConfig // Asegúrate de que izitoastConfig esté definido
            );
          }
        } catch (error) {
          console.error('Error al enviar todos los datos a la API:', error);
          this.$toast.error(
            "Error al enviar los datos. Por favor, inténtalo de nuevo.",
            "Error",
            this.izitoastConfig
          );
        }
      }
    }
  };
  </script>
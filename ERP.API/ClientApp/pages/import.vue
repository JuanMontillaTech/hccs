<template>
  <div>
    <div class="card">

      <div class="card-body">
        <h5 class="card-title">Importar Semestres</h5>
        <p class="card-text">Herramienta para importar los semestres</p>
        <div class="card-body">
          <div class="col-md-6">
              <label for="nombreInstitucion" class="form-label">Año del semestres:</label>
              <input type="text" class="form-control" id="year" v-model="formulario.nombreInstitucion"
                >
            </div>
        
          <div class="row mb-3">
            <div class="col-md-6">
              <label for="nombreInstitucion" class="form-label">Nombre de la Institución:</label>
              <input type="text" class="form-control" id="nombreInstitucion" v-model="formulario.nombreInstitucion"
                >
            </div>
            <div class="col-md-3">
              <label for="codigo" class="form-label">Código:</label>
              <input type="text" class="form-control" id="codigo" v-model="formulario.codigo" >
            </div>
            <div class="col-md-3">
              <label for="numHermanas" class="form-label">No. Hermanas:</label>
              <input type="text" class="form-control" id="numHermanas" v-model="formulario.numHermanas" >
            </div>
          </div>

          <div class="row mb-3">
            <div class="col-md-4">
              <label for="pais" class="form-label">País:</label>
              <input type="text" class="form-control" id="pais" v-model="formulario.pais" >
            </div>
            <div class="col-md-5">
              <label for="ciudad" class="form-label">Ciudad:</label>
              <input type="text" class="form-control" id="ciudad" v-model="formulario.ciudad" >
            </div>
            <div class="col-md-3">
              <label for="numEmpleados" class="form-label">No. Empleados:</label>
              <input type="text" class="form-control" id="numEmpleados" v-model="formulario.numEmpleados" >
            </div>
          </div>

          <div class="row mb-3">
            <div class="col-md-12">
              <label for="direccion" class="form-label">Dirección:</label>
              <input type="text" class="form-control" id="direccion" v-model="formulario.direccion" >
            </div>
          </div>

          <div class="row mb-3">
            <div class="col-md-6">
              <label for="telefono" class="form-label">Teléfono:</label>
              <input type="text" class="form-control" id="telefono" v-model="formulario.telefono" >
            </div>
            <div class="col-md-6">
              <label for="fax" class="form-label">Fax:</label>
              <input type="text" class="form-control" id="fax" v-model="formulario.fax" >
            </div>
          </div>
          <div class="col-md-6">
              <label for="nombreInstitucion" class="form-label">Archivo:</label>
              <input type="file" @change="handleFileUpload" accept=".csv">
            </div>
         
          <button @click="sendData" class="btn btn-primary" v-if="dataForLoad.length">Enviar</button>
          <table v-if="dataForLoad.length" class="table table-striped">
            <thead>
              <tr>
                <th v-for="(header, index) in headers" :key="index">{{ header }}</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(row, index) in dataForLoad" :key="index">
                <td v-for="(value, index) in row" :key="index">{{ value }}</td>
              </tr>
            </tbody>
          </table>

          <button @click="sendData" class="btn btn-primary" v-if="dataForLoad.length">Enviar</button>
        </div>

      </div>
    </div>

  </div>
</template>

<script>
import Papa from 'papaparse';

export default {
  data() {
    return {
      dataForLoad: [],
      headers: [],
      formulario: {
        nombreInstitucion: 'COLEGIO CARDENAL SANCHA',
        codigo: '11020212',
        numHermanas: '5',
        pais: 'COLOMBIA',
        ciudad: 'CÚCUTA - NORTE DE SANTANDER',
        numEmpleados: '48',
        direccion: 'CALLE 22 No. 0B-09 BARRIO BLANCO',
        telefono: '5713803 - 5833744',
        fax: '5713110',
        year: '2021',
      }
    };
  },
  methods: {

    handleFileUpload(event) {
      const file = event.target.files[0];
      Papa.parse(file, {
        header: true,
        dynamicTyping: true,
        complete: (results) => {
          this.headers = results.meta.fields;
          this.dataForLoad = results.data.map(row => ({
            tipo: parseInt(row.TIPO === null || row.TIPO === undefined || row.TIPO === '' ? 0 : row.TIPO),           
            cod: row.COD === null || row.COD === undefined || row.COD === '' ? '' : '' + row.COD,
            cuenta: row.CUENTA === null || row.CUENTA === undefined || row.CUENTA === '' ? '' : row.CUENTA,
            enero: parseFloat(row.ENERO === null || row.ENERO === undefined || row.ENERO === '' ? 0 : row.ENERO),
            febrero: parseFloat(row.FEBRERO === null || row.FEBRERO === undefined || row.FEBRERO === '' ? 0 : row.FEBRERO),
            marzo: parseFloat(row.MARZO === null || row.MARZO === undefined || row.MARZO === '' ? 0 : row.MARZO),
            abril: parseFloat(row.ABRIL === null || row.ABRIL === undefined || row.ABRIL === '' ? 0 : row.ABRIL),
            mayo: parseFloat(row.MAYO === null || row.MAYO === undefined || row.MAYO === '' ? 0 : row.MAYO),
            junio: parseFloat(row.JUNIO === null || row.JUNIO === undefined || row.JUNIO === '' ? 0 : row.JUNIO),
            julio: parseFloat(row.JULIO === null || row.JULIO === undefined || row.JULIO === '' ? 0 : row.JULIO),
            agosto: parseFloat(row.AGOSTO === null || row.AGOSTO === undefined || row.AGOSTO === '' ? 0 : row.AGOSTO),
            septiembre: parseFloat(row.SEPTIEMBRE === null || row.SEPTIEMBRE === undefined || row.SEPTIEMBRE === '' ? 0 : row.SEPTIEMBRE),
            octubre: parseFloat(row.OCTUBRE === null || row.OCTUBRE === undefined || row.OCTUBRE === '' ? 0 : row.OCTUBRE),
            noviembre: parseFloat(row.NOVIEMBRE === null || row.NOVIEMBRE === undefined || row.NOVIEMBRE === '' ? 0 : row.NOVIEMBRE),
            diciembre: parseFloat(row.DICIEMBRE === null || row.DICIEMBRE === undefined || row.DICIEMBRE === '' ? 0 : row.DICIEMBRE),
          }));
        },
      });
    },

    sendData() {
      
      let url = "Import/LoadCSV"
      this.$axios
        .post(url, this.dataForLoad)
        .then((response) => {
          this.$toast.success(
            "Registro guardado.",
            "Notificación"
          );
          result = response;

        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
  },
};
</script>
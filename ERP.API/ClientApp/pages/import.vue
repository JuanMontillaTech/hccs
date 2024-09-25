<template>
  <div>
    <div class="card"  >
     
      <div class="card-body">
        <h5 class="card-title">Importar Semestres</h5>
        <p class="card-text">Herramienta para importar los semestres</p>
        <div class="card-body">
          <input type="file" @change="handleFileUpload" accept=".csv">
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
            tipo: parseFloat(row.TIPO === null || row.TIPO === undefined || row.TIPO === '' ? 0 : row.TIPO),
            tiempo: parseFloat(row.TIEMPO === null || row.TIEMPO === undefined || row.TIEMPO === '' ? 0 : row.TIEMPO),
            cod: parseFloat(row.COD === null || row.COD === undefined || row.COD === '' ? 0 : row.COD),
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
      console.log(this.dataForLoad)
      let url = "TransactionReceipt/LoadCSV"
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
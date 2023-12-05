<script>
/**
 * Loader component
 */
import { tsNullKeyword } from "@babel/types";
var numbro = require("numbro");
var moment = require("moment");
export default {
  data: () => ({
    company: {},
  }),
  props: ["title"],
  created() {
    this.getCompany();
  },
  methods: {
    getCompany() {
      let url =  `Company/GetDefault`;

      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.company = response.data.data;
        })
        .catch((error) => {
          console.log(error);
        });
    },
  },
};
</script>

<template>

  <div >
<table class="w-100">
  <tr>
    <th>Institución:</th> <td>{{ company.companyName }}</td>  <th>Ciudad o Pueblo:</th> <td>{{ company.town }}</td> <th>Código:</th> <td>{{ company.collaborators }}</td>

  </tr>
  <tr>
    <th>País:</th> <td>{{ company.country }}</td>  <th>Teléfono :</th> <td>{{ company.phones }}</td> <th>No. De Hermanas:</th> <td>{{ company.systers }}</td>

  </tr>
  <tr>
    <th>Dir:</th> <td>{{  company.address }}</td>  <th>Fecha :</th> <td>{{ company.phones }}</td> <th>No. De Empleados:</th> <td>{{ company.employees }}</td>

  </tr>
</table>

  </div>
</template>

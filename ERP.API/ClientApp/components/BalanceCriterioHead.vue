<script>
/**
 * Loader component
 */
import { tsNullKeyword } from "@babel/types";
var numbro = require("numbro");
var moment = require("moment");
export default {
  data: () => ({
    totalbalance: 0.0,
  }),
  props: ["List", "type"],
  methods: {
    calculatebalance() {
      this.totalbalance = this.List.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);
    },
    dataFilter(type) {
      var dataFillter = this.List.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);
      return dataFillter;
    },
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    GetDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },
  },
};
</script>

<template>
  <div>
    <div v-if="List.length == 0">
          <div class="spinner">
            <i class="uil-shutter-alt spin-icon"></i>
          </div>
    </div>
    <table
      v-if="List.length > 0"
      class="table table-nowrap table-centered mb-0"
    >
      <thead>
        <tr>
          <th>Cuenta</th>
          
        </tr>
      </thead>
      <tbody>
        <tr v-for="row in dataFilter(type)" :key="row.ledgerAccountId">
          <td>
            <h5 class="font-size-14 mb-1">
              {{ row.code }} {{ row.name }}
              
            </h5>
          </td>
          
        </tr>

       
      </tbody>
    </table>
  </div>
</template>

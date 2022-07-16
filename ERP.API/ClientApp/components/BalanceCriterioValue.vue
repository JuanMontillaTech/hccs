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
         
          <th class="text-right" style="width: 120px"> </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="row in dataFilter(type)" :key="row.ledgerAccountId">
        
          <td class="border-0 text-right">{{ SetTotal(row.balance) }}</td>
        </tr>

        <tr>
           
          <td class="border-0 text-right doubleLine">
            <div class="doubleLine">{{ SetTotal(totalbalance) }}</div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

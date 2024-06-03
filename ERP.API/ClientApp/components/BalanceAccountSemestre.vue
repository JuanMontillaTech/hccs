<script>
/**
 * Loader component
 */

var numbro = require("numbro");


export default {
  data: () => ({
    company: {},
  }),
  props: ["Balances"],

  methods: {
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    getMonthValue(monthNumber, Months) {
      const monthData = Months.find(month => month.month === monthNumber);
      if (monthData) {
        return monthData.value;
      } else {
        return 0.0;
      }
    },
    getTotalForCode(data, code) {
      let total = 0;
      for (const balance of data) {
        if (balance.Code === code) {
          for (const month of balance.Months) {
            total += month.value;
          }
        }
      }
      return total;
    },
  },
};
</script>

<template>
  <div  >
<table   >
  <thead   >
  <tr>
    <th>
      41
    </th>
    <th>
      INGRESOS
    </th>
    <td></td>
    <td></td>
    <td></td>
    <td></td>
    <td></td>
    <td></td>
    <td></td>

  </tr>
  </thead>
  <tbody>
    <tr   class="w-100" v-for="row in Balances" :key="row.Code">
      <Td>
        {{ row.Code }}
      </Td>
      <td>
        {{ row.Name }}
      </td>
      <td>
        {{ SetTotal(getMonthValue(1, row.Months)) }}
      </td>
      <td>
        {{ SetTotal(getMonthValue(2, row.Months)) }}
      </td>
      <td>
        {{ SetTotal(getMonthValue(3, row.Months)) }}
      </td>
      <td>
        {{ SetTotal(getMonthValue(4, row.Months)) }}
      </td>
      <td>
        {{ SetTotal(getMonthValue(5, row.Months)) }}
      </td>
      <td>
        {{ SetTotal(getMonthValue(6, row.Months)) }}
      </td>
      <td>
        {{ SetTotal(getTotalForCode(Balances, row.Code)) }}
      </td>
    </tr>
  </tbody>
</table>
  </div>
</template>

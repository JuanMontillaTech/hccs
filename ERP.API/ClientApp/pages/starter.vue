<script>
/**
 * Starter page
 */
export default {
  head() {
    return {
      title: `${this.title} | `,
    };
  },
  data() {
    return {
      title: "Bienvenido",
      items: [
        {
          text: " ",
          active: true,
        },
      ],

      stackBarChart: {
        options: {
          stackBars: true,
          axisY: {
            labelInterpolationFnc: (value) => {
              return value / 1000 + "$";
            },
          },
          height: 300,
        },
      },
      dataSales: {
        labels: [
          "Ene",
          "Feb",
          "Mar",
          "Abr",
          "May",
          "Jun",
          "Jul",
          "Agos",
          "Sep",
          "Oct",
          "Nove",
          "Dic",
        ],
        series: [[1000, 21000, 91000, 31000, 51000, 61000]],
      },
    };
  },
  middleware: "authentication",
};
</script>

<template>
  <div>
    <div class="row">
      <div class="col-lg-6">
        <div class="card">
          <div class="card-body" dir="ltr">
            <h4 class="card-title mb-4">Ventas x Mes</h4>

            <!-- Stacked bar chart -->
            <chartist
              ratio="ct-chart"
              :data="dataSales"
              :options="stackBarChart.options"
              type="Bar"
            ></chartist>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

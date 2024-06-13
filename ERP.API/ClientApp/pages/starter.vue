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
<<<<<<< HEAD
  <div>
=======
  
  <div> 
>>>>>>> b303e1b38a79dc9aca14bbf7cf0d380711f10e26
    <div class="row">
      <div class="col-lg-6">
        <div class="card">
          <div class="card-body" dir="ltr">
            <DynamicStatistic></DynamicStatistic>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
 
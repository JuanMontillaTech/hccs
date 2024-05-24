<script>
import { Doughnut } from "vue-chartjs";
import { Colerslist } from "../helpers/colors2";

export default {
  extends: Doughnut,
  props: {
    labels: {
      type: Array,
      required: true,
    },
    dataDashboard: {
      type: Array,
      required: true,
    },
  },
  mounted() {
    this.renderChart(
      {
        labels: this.labels,
        datasets: [
          {
            data: this.dataDashboard,
            backgroundColor: Colerslist,
            hoverBackgroundColor:Colerslist,

            hoverBorderColor: "#fff",
            borderWidth: 0,
            radius: "90%",
            innerRadius: "75%",
          },
        ],
      },
      {
        maintainAspectRatio: true,
      }
    );
  },
};
</script>

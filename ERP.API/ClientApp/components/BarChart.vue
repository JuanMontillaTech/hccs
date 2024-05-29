<script>
import { Bar } from "vue-chartjs";
import { Colerslist } from "../helpers/colors";
export default {
  extends: Bar,
  props: {
    labels: {
      type: Array,
      required: true,
    },
    dataDashboard: {
      type: Array,
      required: true,
    },
    bgColor: {
      type: String,
      required: true,
    },
    bgHoveColor: {
      type: String,
      required: true,
    },
    titleHeader: {
      type: String,
      required: true,
    },
  },
  mounted() {
    this.renderChart(
      {
        labels: this.labels,
        datasets: [
          {
            barPercentage: 0.4,
            label: this.titleHeader,
            borderColor: this.bgColor,
            borderWidth: 1,
            hoverBorderColor: this.bgHoveColor,
            data: this.dataDashboard,
            backgroundColor: Colerslist,
            hoverBackgroundColor:Colerslist,
          },
        ],
      },
      {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          xAxes: [
            {
              gridLines: {
                color: "rgba(166, 176, 207, 0.1)",
              },
            },
          ],
          yAxes: [
            {
              gridLines: {
                color: "rgba(166, 176, 207, 0.1)",
              },
            },
          ],
        },
      }
    );
  },
};
</script>

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
      let url = `Company/GetDefault`;

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
  <div  style="background-color: white">
    <h4 class="float-end font-size-16">{{ title }}</h4>
    <div class="row">
      <div class="col-sm-3">
        <div class="card" style="box-shadow: none !important">
          <div class="card-body" style="box-shadow: none !important">
            <img
              src="~/assets/images/logo-smsancha.png"
              alt=""
              style="width: 100px; height: 100px"
            />
          </div>
        </div>
      </div>
      <div class="col-sm-6">
        <div class="card" style="box-shadow: none !important">
          <div class="card-body" style="box-shadow: none !important">
            <div class="text-muted">
              <p class="font-size-16">{{ company.companyName }}</p>
              <p class="mb-1">{{ company.address }}</p>
              <p class="mb-1">
                <i class="uil uil-envelope-alt mr-1"></i> {{ company.email }}
              </p>
              <p><i class="uil uil-phone mr-1"></i> {{ company.phones }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

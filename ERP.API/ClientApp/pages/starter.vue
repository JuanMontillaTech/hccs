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
      formId: '61F249CF-D923-4B8F-BF80-160B5C7F5AE8',
      items: [
        {
          text: " ",
          active: true,
        },
      ],
     misLinks: [
 
],

  
      
    };
  },
  created() {
    this.getLinks();
  },
  methods: {
    async getLinks() {
       
      const url = `QuickLinks/GetAll`;
      try {
        const response = await this.$axios.get(url); 
        this.misLinks = response.data.data || [];
      } catch (error) {
        this.misLinks = [];
      }
    },
  },
};
</script>

<template>
 
  <div>
 
  
 
    <div class="row">
      <div class="col-lg-4" v-if="misLinks">
        <div class="card">
          <div class="card-body" dir="ltr">
         
            <Quicklinks :links="misLinks" >  </Quicklinks>
          </div>
        </div>
      </div>
    
    </div>

    <div class="row">
    
      <div class="col-lg-12">
         
            <DynamicStatistic :FormId="formId"></DynamicStatistic>
            
         
      </div>
    </div>
  </div>
</template>
 
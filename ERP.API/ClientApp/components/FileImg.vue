<template>
  <div>
  
    <img  v-if="file" :src="file.link"  class="avatar-lg rounded-circle img-thumbnail"  />
  
  </div>
</template>

<script>
 
export default {
  name: "FileImg",

  data: () => ({
    
   
    file: null,
 
    
  }),
  props: ["SourceId"],
 
  mounted() {
 
    if(this.SourceId !=undefined){
     
        this.GetFile();
    }
     
  },
  methods: {

    GetFile() {
      this.$axios
        .get(`FileManager/GetBySourceIdFirst?SourceId=${this.SourceId}`)
        .then((response) => {
          this.file = response.data.data;
         
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
 
  },
};
</script>

<style scoped>
.loader {
  text-align: center;
  color: #bbbbbb;
}
</style>

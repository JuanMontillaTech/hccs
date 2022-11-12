<template>
<div>
<!-- <ListDynamicExpress> </ListDynamicExpress> -->
<Datatable      :includeNewOption="true"/>
</div>
</template>
<script>
 
export default {
 
  
  data() {
    return {
      customElements: [
     
        {
          title: "Editar",
          icon: "uil uil-pen font-size-18",
          styleIcon: "px-2 text-success",
          link: "/FuncionalFormExpress",
        },
      ],
      
    };
  },
}
</script>





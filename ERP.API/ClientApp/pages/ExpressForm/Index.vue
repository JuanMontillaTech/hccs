<template>
<div>
<!-- <ListDynamicExpress> </ListDynamicExpress> -->
<Datatable   :customLinks="customElements"  :includeNewOption="true"/>
</div>
</template>
<script>
 
export default {
 
  
  data() {
    return {
      customElements: [
        {
          title: "Ver",
          icon: "uil uil-eye font-size-18",
          styleIcon: "px-2 text-success",
          link: "/citizen/citizen-request-details",
        },
        {
          title: "Editar",
          icon: "uil uil-pencil font-size-18",
          styleIcon: "px-2 text-success",
          link: "/FuncionalFormExpress",
        },
      ],
      
    };
  },
}
</script>





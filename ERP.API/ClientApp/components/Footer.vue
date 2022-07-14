<script>
/**
 * Footer component
 */
export default {};
</script>

<template>
  <footer class="footer">
    <div class="container-fluid">
      <div class="row">
        <div class="col-sm-6">{{ new Date().getFullYear() }} © Cardenal Sancha.</div>
        <div class="col-sm-6">
          <div class="text-sm-end d-none d-sm-block">
            © {{ new Date().getFullYear() }} Creado con
            <i class="mdi mdi-heart text-danger"></i> por
            <a
              class="text-reset"
              href="https://api.whatsapp.com/send/?phone=18293087380&text=Hola Juan!&app_absent=0"
              target="_blank"
              ><i class="fab fa-whatsapp"> </i>Juan Montilla</a
            >
          </div>
        </div>
      </div>
    </div>
  </footer>
</template>

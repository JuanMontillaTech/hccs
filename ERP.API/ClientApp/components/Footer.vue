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
        <div class="col-sm-6">API V1 , Client V1</div>

        <div class="col-sm-6">
          <div class="text-sm-end d-none d-sm-block">
            © {{ new Date().getFullYear() }} Soporte

            <a
              class="text-reset"
              href="https://api.whatsapp.com/send/?phone=18293087380&text=Hola Juan!&app_absent=0"
              target="_blank"
              ><i class="fab fa-whatsapp"> </i> 809-328-9538 Juan Montilla</a
            >
          </div>
        </div>
      </div>
    </div>
  </footer>
</template>

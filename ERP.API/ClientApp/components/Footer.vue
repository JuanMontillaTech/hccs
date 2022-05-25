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
            <div class="col-sm-6">{{ new Date().getFullYear()}} © Minible.</div>
            <div class="col-sm-6">
                <div class="text-sm-end d-none d-sm-block">
                    Creado por
                    <i class="mdi mdi-heart text-danger"></i> by
                    <a  class="text-reset">Juan Montilla</a>
                </div>
            </div>
        </div>
    </div>
</footer>
</template>

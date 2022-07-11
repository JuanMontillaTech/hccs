<template>
  <FuntionalList
    Title="Facturas de compras"
    TransactionsType="2"
    Form="facturascompras"
    IsClient="false"
    DateLabel="Fecha de compra"
    :Path="$route.path"
     DocumentTypeId="10"
  ></FuntionalList>
</template>

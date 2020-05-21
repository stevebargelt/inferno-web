<script>
  import Tailwindcss from "./Tailwindcss.svelte";

  export let name;
</script>

<style>
  h1 {
    @apply bg-black text-white;
  }
</style>

<Tailwindcss />
<h1>Hello {name}!</h1>
{(async function() {
  let { text } = await (await fetch(`/api/status`)).json();
  document.querySelector('#name').textContent = text;
})()}

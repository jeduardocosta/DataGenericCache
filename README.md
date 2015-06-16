# data-generic-cache

[![Build status](https://ci.appveyor.com/api/projects/status/c806rq2m850k5j1v?svg=true)](https://ci.appveyor.com/project/jeduardocosta/data-generic-cache) [![Coverage Status](https://coveralls.io/repos/jeduardocosta/data-generic-cache/badge.svg)](https://coveralls.io/r/jeduardocosta/data-generic-cache)


A simple library to cache data using custom providers.

Available providers
----------
- Local memory
- Redis

Configuration
----------
Use custom config section named "dataGenericCacheSection" in configuration file.

```xml
<dataGenericCacheSection>
   <providers>
      <provider type="redis" server="remote-redis" port="6379" password="remote-redis-password" />
      <provider type="redis" server="local-redis" port="6379" />
      <provider type="localmemory" />
   </providers>
   <activeProviderCacheInMinutes value="60" />
</dataGenericCacheSection>
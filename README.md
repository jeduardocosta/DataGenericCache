# data-generic-cache
A simple library to cache data using custom providers.

Available providers
----------
- Local memory
- Redis

Configuration
----------
Use custom config section named "dataGenericCacheSection" in configuration file.

<dataGenericCacheSection>
   <providers>
      <provider type="redis" server="remote-redis" port="6379" password="remote-redis-password" />
      <provider type="redis" server="local-redis" port="6379" />
      <provider type="localmemory" />
   </providers>
   <activeProviderCacheInMinutes value="60" />
</dataGenericCacheSection>
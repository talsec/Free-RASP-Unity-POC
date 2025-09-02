# we need to keep the controller class from being obfuscated as
# it needs to be called from c#
-keep class com.unity.free.rasp.Controller { public *; }
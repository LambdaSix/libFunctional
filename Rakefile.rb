require 'fileutils'
require 'rake/clean'
require 'albacore'

PRODUCT = "libFunctional"

CLEAN.include('libFunctional/bin/*')

task :default do
	puts "#{PRODUCT} rake file:"
	puts %x{rake -D}
end

desc "Build #{PRODUCT}"
msbuild :build, [:config] do |msb, args|
	msb.properties = { :configuration => args.config }
	msb.targets = [:Build]
	msb.solution = 'libFunctional.sln'
end
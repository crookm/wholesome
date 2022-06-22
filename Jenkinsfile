pipeline {
    agent any
    
    stages {
        stage('build') {
            steps {
                dotnetRestore project: 'Wholesome.sln'
                dotnetBuild configuration: 'Release', noRestore: true
            }
            
            post {
                success {
                    archiveArtifacts artifacts: '**/*.nupkg', followSymlinks: false
                }
            }
        }
        
        stage('test') {
            steps {
                dotnetTest configuration: 'Release', noBuild: true, logger: 'trx;test_results.xml', collect: 'XPlat Code Coverage'
            }
            
            post {
                always {
                    xunit followSymlink: false, reduceLog: false, tools: [xUnitDotNet(excludesPattern: '', pattern: '**/test_results.xml', stopProcessingIfError: true)]
                    cobertura coberturaReportFile: '**/coverage.cobertura.xml'
                }
            }
        }
    }
}
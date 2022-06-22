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
                dotnetTest configuration: 'Release', noBuild: true, logger: '"trx;LogFileName=test_results.trx"', collect: 'XPlat Code Coverage'
            }
            
            post {
                always {
                    xunit followSymlink: false, reduceLog: false, tools: [MSTest(excludesPattern: '', pattern: '**/test_results.trx', stopProcessingIfError: true)]
                    cobertura coberturaReportFile: '**/coverage.cobertura.xml'
                }
            }
        }
        
        stage('publish') {
            steps {
                dotnetNuGetPush root: '**/*.nupkg', source: 'https://www.nuget.org', apiKeyId: '2d95a085-0adc-474b-b7f2-d661fe005c08', skipDuplicate: true
            }
        }
    }
}
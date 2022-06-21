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
                dotnetTest configuration: 'Release', noBuild: true, collect: 'XPlat Code Coverage'
            }
            
            post {
                always {
                    cobertura coberturaReportFile: '**/coverage.cobertura.xml'
                }
            }
        }
    }
}